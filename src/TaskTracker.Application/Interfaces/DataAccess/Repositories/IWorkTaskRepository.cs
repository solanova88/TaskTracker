using TaskTracker.Application.Interfaces.DataAccess.Repositories.Common;
using TaskTracker.Domain.Enums;
using TaskTracker.Domain.Models.Tasks;

namespace TaskTracker.Application.Interfaces.DataAccess.Repositories;

public interface IWorkTaskRepository : IRepository<WorkTask>
{
	Task<WorkTask?> GetWithSubtasksAndRelationsAsync(Guid id, bool readOnly = false);
	Task<List<WorkTask>> GetListWithSubtasksAndRelationsAsync(
		string? author = null,
		string? assignee = null,
		WorkTaskStatus? status = null,
		WorkTaskPriority? priority = null,
		bool? hasParent = null,
		int skip = 0,
		int take = 10,
		bool readOnly = true);
	Task<WorkTask?> GetWithRelationsAsync(Guid id, bool readOnly = false);
}