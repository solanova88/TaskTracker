using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using TaskTracker.Domain.Common;

namespace TaskTracker.Application.Interfaces.DataAccess.Repositories.Common;

public interface IRepository<TEntity> where TEntity : IEntity
{
    Task<TEntity?> GetByIdAsync(Guid id, bool readOnly = false);
    
    Task AddAsync(TEntity entity);
    
    Task AddRangeAsync(IEnumerable<TEntity> entities);

    Task<int> ExecuteUpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls);
    
    void Remove(TEntity entity);
    
    void RemoveRange(IEnumerable<TEntity> entities);
}