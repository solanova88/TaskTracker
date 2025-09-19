using Microsoft.EntityFrameworkCore;
using TaskTracker.Application.Interfaces.DataAccess.Repositories;
using TaskTracker.Domain.Enums;
using TaskTracker.Domain.Models.Tasks;
using TaskTracker.Infrastructure.ApplicationDependencies.DataAccess.Repositories.Common;
using TaskTracker.Infrastructure.Persistence.Context;

namespace TaskTracker.Infrastructure.ApplicationDependencies.DataAccess.Repositories;

public class WorkTaskRepositoryEF : RepositoryBaseEF<WorkTask>, IWorkTaskRepository
{
	public WorkTaskRepositoryEF(TaskTrackerDbContext context) : base(context)
	{
	}

	protected override IQueryable<WorkTask> BaseQuery 
		=> _context.Tasks;
	
	public async Task<WorkTask?> GetWithSubtasksAndRelationsAsync(Guid id, bool readOnly = false)
	{
		var query = BaseQuery
			.Include(t => t.Subtasks)
			.Include(t => t.RelatedTo)
			.Include(t => t.RelatedFrom)
			.Where(t => t.Id == id);

		if (readOnly)
			query = query.AsNoTracking();

		return await query.FirstOrDefaultAsync().ConfigureAwait(false);
	}
	
	public async Task<List<WorkTask>> GetListWithSubtasksAndRelationsAsync(
		string? author = null,
		string? assignee = null,
		WorkTaskStatus? status = null,
		WorkTaskPriority? priority = null,
		bool? hasParent = null,
		int skip = 0,
		int take = 10,
		bool readOnly = true)
	{
		IQueryable<WorkTask> query = BaseQuery
			.Include(t => t.Subtasks)
			.Include(t => t.RelatedTo)
			.Include(t => t.RelatedFrom);

		if (!string.IsNullOrEmpty(author))
			query = query.Where(t => t.Author == author);

		if (!string.IsNullOrEmpty(assignee))
			query = query.Where(t => t.Assignee == assignee);

		if (status.HasValue)
			query = query.Where(t => t.Status == status.Value);

		if (priority.HasValue)
			query = query.Where(t => t.Priority == priority.Value);

		if (hasParent.HasValue)
		{
			query = hasParent.Value 
				? query.Where(t => t.ParentTaskId.HasValue) 
				: query.Where(t => t.ParentTaskId == null);
		}

		if (readOnly)
			query = query.AsNoTracking();

		return await query
			.OrderByDescending(t => t.CreatedDate)
			.Skip(skip)
			.Take(take)
			.ToListAsync()
			.ConfigureAwait(false);
	}

	public async Task<WorkTask?> GetWithRelationsAsync(Guid id, bool readOnly = false)
	{
		var query = BaseQuery
			.Include(t => t.Subtasks)
			.Include(t => t.RelatedTo)
			.Include(t => t.RelatedFrom)
			.Where(t => t.Id == id);

		if (readOnly)
			query = query.AsNoTracking();

		return await query.FirstOrDefaultAsync();
	}
}