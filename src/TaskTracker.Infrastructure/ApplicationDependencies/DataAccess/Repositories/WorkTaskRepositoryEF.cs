using TaskTracker.Application.Interfaces.DataAccess.Repositories;
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
}