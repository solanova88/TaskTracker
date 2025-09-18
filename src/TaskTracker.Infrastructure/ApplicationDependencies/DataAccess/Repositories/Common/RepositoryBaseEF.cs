using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TaskTracker.Application.Interfaces.DataAccess.Repositories;
using TaskTracker.Application.Interfaces.DataAccess.Repositories.Common;
using TaskTracker.Domain.Common;
using TaskTracker.Infrastructure.Persistence.Context;

namespace TaskTracker.Infrastructure.ApplicationDependencies.DataAccess.Repositories.Common;

public abstract class RepositoryBaseEF<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
{
    protected readonly TaskTrackerDbContext _context;
    protected readonly DbSet<TEntity> _set;

    /// <summary>
    /// Defines the base query for the given entity used by all operations.
    /// Concrete implementations should apply all necessary includes and pre-filters here.
    /// </summary>
    protected abstract IQueryable<TEntity> BaseQuery { get; }

    protected RepositoryBaseEF(TaskTrackerDbContext context)
    {
        _context = context;
        _set = context.Set<TEntity>();
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id, bool readOnly = false)
        => await (readOnly ? BaseQuery.AsNoTracking() : BaseQuery).FirstOrDefaultAsync(e => e.Id == id);

    public virtual async Task AddAsync(TEntity entity)
        => await _set.AddAsync(entity);

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
        => await _set.AddRangeAsync(entities);
    
    public virtual Task<int> ExecuteUpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls) 
        => BaseQuery.Where(predicate).ExecuteUpdateAsync(setPropertyCalls);

    public virtual void Remove(TEntity entity)
    {
        _context.Remove(entity);
    }

    public virtual void RemoveRange(IEnumerable<TEntity> entities)
    {
        foreach (var e in entities)
            Remove(e);
    }
}