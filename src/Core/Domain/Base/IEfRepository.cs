using System.Linq.Expressions;

namespace Core.Domain.Base;

public interface IEfRepository<TEntity> : IRepository where TEntity : Entity
{
    void Create(TEntity aggregate);
    void Update(TEntity aggregate);
    void Delete(TEntity aggregate);
    Task<TEntity> FindByIdAsync(Guid key, Expression<Func<TEntity, bool>> predicate = null);
    IQueryable<TEntity> FindAllAsQueryable(params Expression<Func<TEntity, object>>[] includeProperties);
    Task<TEntity> FindOneByExpression(Expression<Func<TEntity, bool>> predicate);
    Task<bool> IsExistsAsync(Expression<Func<TEntity, bool>> predicate);
}