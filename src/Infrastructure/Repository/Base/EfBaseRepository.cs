using Core.Data;
using Core.Domain.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repository.Base;

public class EfBaseRepository<TEntity> : IEfRepository<TEntity> where TEntity : Entity
{
    protected readonly Context Context;
    protected readonly DbSet<TEntity> DbSet;

    public EfBaseRepository(Context context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    public virtual void Create(TEntity aggregate)
    {
        DbSet.Add(aggregate);
    }

    public virtual void Delete(TEntity aggregate)
    {
        DbSet.Remove(aggregate);
    }

    public IQueryable<TEntity> FindAllAsQueryable(params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var q = DbSet.AsNoTracking().AsQueryable();

        if (includeProperties is not null)
        {
            foreach (var includeProperty in includeProperties)
            {
                q = q.Include(includeProperty);
            }
        }

        return q;
    }

    public virtual async Task<TEntity> FindByIdAsync(Guid key, Expression<Func<TEntity, bool>> predicate = null)
    {
        var q = DbSet.Where(a => a.Id.Equals(key));

        if (predicate != null) q = q.Where(predicate);

        return await q.AsNoTracking().FirstOrDefaultAsync();
    }

    public virtual async Task<TEntity> FindOneByExpression(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbSet.Where(predicate).AsNoTracking().FirstOrDefaultAsync();
    }

    public virtual async Task<bool> IsExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbSet.AsNoTracking().AnyAsync(predicate);
    }

    public virtual void Update(TEntity aggregate)
    {
        aggregate.SetUpdatedAt(DateTime.UtcNow);
        DbSet.Update(aggregate);
    }
}