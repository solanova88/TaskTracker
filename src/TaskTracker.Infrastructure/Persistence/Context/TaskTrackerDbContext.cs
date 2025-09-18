using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TaskTracker.Application.Interfaces.Common;
using TaskTracker.Domain.Common;
using TaskTracker.Domain.Models.Tasks;
using TaskTracker.Infrastructure.Persistence.Context.ValueConverters;

namespace TaskTracker.Infrastructure.Persistence.Context;

public sealed class TaskTrackerDbContext : DbContext
{
	private readonly IDateTime _dateTime;

	public TaskTrackerDbContext(DbContextOptions<TaskTrackerDbContext> options, IDateTime dateTime) : base(options)
	{
		_dateTime = dateTime;
	}
	
	public DbSet<WorkTask> Tasks => Set<WorkTask>();
	public DbSet<WorkTaskRelation> TaskRelations => Set<WorkTaskRelation>();
	
	protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
	{
		configurationBuilder
			.Properties<DateTime>()
			.HaveConversion(typeof(UtcValueConverter));
	}
	
	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.Entity<WorkTask>()
			.Property(u => u.Id)
			.ValueGeneratedNever();
		
		builder.Entity<WorkTask>().HasQueryFilter(x => !x.IsDeleted);
		
		builder.Entity<WorkTaskRelation>()
			.HasQueryFilter(r => !r.WorkTask.IsDeleted && !r.RelatedWorkTask.IsDeleted);
		
		builder.Entity<WorkTaskRelation>()
			.HasKey(r => new { r.WorkTaskId, r.RelatedWorkTaskId });

		builder.Entity<WorkTaskRelation>()
			.HasOne(r => r.WorkTask)
			.WithMany(t => t.RelatedTo)
			.HasForeignKey(r => r.WorkTaskId)
			.OnDelete(DeleteBehavior.Restrict);


		builder.Entity<WorkTaskRelation>()
			.HasOne(r => r.RelatedWorkTask)
			.WithMany(t => t.RelatedFrom)
			.HasForeignKey(r => r.RelatedWorkTaskId)
			.OnDelete(DeleteBehavior.Restrict);
		
		foreach (var entityType in builder.Model.GetEntityTypes())
		{
			foreach (var property in entityType.GetProperties())
			{
				var type = property.ClrType;
				Type? enumType = null;

				if (type.IsEnum)
				{
					enumType = type;
				}
				else
				{
					var underlying = Nullable.GetUnderlyingType(type);
					if (underlying is { IsEnum: true })
						enumType = underlying;
				}

				if (enumType == null)
					continue;
				
				var converterType = typeof(EnumToStringConverter<>).MakeGenericType(enumType);
				var converter = (ValueConverter)Activator.CreateInstance(converterType, [null])!;

				property.SetValueConverter(converter);
				property.SetMaxLength(50);
				
				property.IsNullable = Nullable.GetUnderlyingType(type) != null;
			}
		}
	}
	
	public override int SaveChanges()
	{
		OnBeforeSaving();
		return base.SaveChanges();
	}

	public override int SaveChanges(bool acceptAllChangesOnSuccess)
	{
		OnBeforeSaving();
		return base.SaveChanges(acceptAllChangesOnSuccess);
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		OnBeforeSaving();
		return base.SaveChangesAsync(cancellationToken);
	}

	public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
		CancellationToken cancellationToken = default)
	{
		OnBeforeSaving();
		return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
	}
	
	public Task<int> SaveChangesWithDeletedAsync(CancellationToken cancellationToken = default)
	{
		OnBeforeSaving();
		return base.SaveChangesAsync(acceptAllChangesOnSuccess: true, cancellationToken);
	}
	/// <summary>
	/// Automatically stores metadata when entities are added, modified, or deleted.
	/// </summary>
	private void OnBeforeSaving()
	{
		var utcNow = _dateTime.UtcNow;

		foreach (var entry in ChangeTracker.Entries())
		{
			if (entry is { Entity: ISoftDeletable, State: EntityState.Deleted })
			{
				entry.State = EntityState.Unchanged; // Override removal. Better than Modified, because that flags ALL properties for update.
				entry.Property(nameof(ISoftDeletable.IsDeleted)).CurrentValue = true;
				entry.Property(nameof(ISoftDeletable.DeletedDate)).CurrentValue = utcNow;
			}

			if (entry.Entity is not IAuditable) continue;
			
			switch (entry.State)
			{
				case EntityState.Added:
					entry.Property(nameof(IAuditable.CreatedDate)).CurrentValue = utcNow;
					break;
				case EntityState.Modified:
					entry.Property(nameof(IAuditable.UpdatedDate)).CurrentValue = utcNow;
					break;
			}
		}
	}
}