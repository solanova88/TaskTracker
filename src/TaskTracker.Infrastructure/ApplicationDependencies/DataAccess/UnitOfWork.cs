using Microsoft.EntityFrameworkCore.Storage;
using TaskTracker.Application.Interfaces.DataAccess;
using TaskTracker.Application.Interfaces.DataAccess.Repositories;
using TaskTracker.Infrastructure.Persistence.Context;

namespace TaskTracker.Infrastructure.ApplicationDependencies.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly TaskTrackerDbContext _dbContext;
    private IDbContextTransaction _currentTransaction;
    private bool _disposed;
    
    public IWorkTaskRepository Tasks { get; set; }

    public UnitOfWork(TaskTrackerDbContext dbContext, IWorkTaskRepository workTaskRepository)
    {
        _dbContext = dbContext;
        Tasks = workTaskRepository;
    }
    
    ~UnitOfWork()
    {
        Dispose(false);
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            _dbContext.Dispose();
            _currentTransaction.Dispose();
        }

        _disposed = true;
    }
    
    public async ValueTask DisposeAsync()
    {
        if (!_disposed)
        {
            if (_currentTransaction is not null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }

            if (_dbContext is not null)
            {
                await _dbContext.DisposeAsync();
            }

            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }
    
    public Task SaveChangesAsync() =>
        _dbContext.SaveChangesWithDeletedAsync();
    
    public bool HasActiveTransaction =>
        _currentTransaction is not null;
    
    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        if (_currentTransaction is not null)
        {
            throw new InvalidOperationException("Транзакция уже начата.");
        }

        _currentTransaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        return _currentTransaction;
    }
    
    public async Task CommitTransactionAsync()
    {
        try
        {
            await _dbContext.SaveChangesAsync();

            if (_currentTransaction is not null)
            {
                await _currentTransaction.CommitAsync();
            }
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            if (_currentTransaction is not null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }
    
    public async Task RollbackTransactionAsync()
    {
        if (_currentTransaction is null)
        {
            throw new InvalidOperationException("Нет активной транзакции для отката.");
        }

        try
        {
            await _currentTransaction.RollbackAsync();
        }
        finally
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }
}