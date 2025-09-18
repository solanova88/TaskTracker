using Microsoft.EntityFrameworkCore.Storage;
using TaskTracker.Application.Interfaces.DataAccess.Repositories;

namespace TaskTracker.Application.Interfaces.DataAccess;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
	public IWorkTaskRepository Tasks { get; }
	
	bool HasActiveTransaction { get; }

	Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
	Task CommitTransactionAsync();
	Task RollbackTransactionAsync();
	public Task SaveChangesAsync();
}