using Microsoft.EntityFrameworkCore;
using TaskTracker.Infrastructure.Persistence.Context;

namespace TaskTracker.Api.Startup;

public static class DbContextSetup
{
	public static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("DefaultConnection");
		services.AddDbContext<TaskTrackerDbContext>(options =>
			options.UseNpgsql(connectionString, sql => 
				sql.MigrationsAssembly(typeof(TaskTrackerDbContext).Assembly.FullName)));

		return services;
	}

	public static async Task ApplyMigrationsAsync(this IApplicationBuilder app, IConfiguration configuration)
	{
		using var scope = app.ApplicationServices.CreateScope();
		var services = scope.ServiceProvider;

		try
		{
			var context = services.GetRequiredService<TaskTrackerDbContext>();

			// Применяем миграции, если включена настройка AutoMigrations
			if (configuration.GetValue<bool>("AutoMigrations"))
			{
				await context.Database.MigrateAsync();
			}
		}
		catch (Exception exception)
		{
			var logger = scope.ServiceProvider.GetRequiredService<ILogger<TaskTrackerDbContext>>();

			logger.LogError(exception, "An error occurred while migrating");

			throw;
		}
	}
}