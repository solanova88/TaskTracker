using TaskTracker.Application.Interfaces.DataAccess.Repositories.Common;
using TaskTracker.Domain.Models.Tasks;

namespace TaskTracker.Application.Interfaces.DataAccess.Repositories;

public interface IWorkTaskRepository :  IRepository<WorkTask>;