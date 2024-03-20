using System.Linq.Expressions;
using HomeStation.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace HomeStation.Application.Common.Virtual;

public class Repository<TEntity> where TEntity : class
{
    private IAirDbContext _dbContext;
    private DbSet<TEntity> _dbSet;
    
    public Repository(IAirDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<TEntity>();
    }

    /// <summary>
    /// Deletes from repository
    /// </summary>
    /// <param name="entity">Entity</param>
    public virtual void Delete(TEntity? entity)
    {
        if (_dbContext.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }
        _dbSet.Remove(entity);
    }

    /// <summary>
    /// Gets entities by filter
    /// </summary>
    /// <param name="predicate">Expression filter</param>
    /// <param name="includeProperties">Properties</param>
    /// <returns>IEnumerable of entities</returns>
    public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var query = _dbSet.AsQueryable();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
        }

        return query;
    }
    
    /// <summary>
    /// Gets all entities
    /// </summary>
    /// <param name="includeProperties">Properties</param>
    /// <returns>IEnumerable of entities</returns>
    public virtual IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var query = _dbSet.AsQueryable();

        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
        }

        return query;
    }
    
    /// <summary>
    /// Gets single entity object by filter
    /// </summary>
    /// <param name="filter">Expression filter</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Entity</returns>
    public virtual async Task<TEntity?> GetObjectBy(Expression<Func<TEntity?, bool>> filter, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties)
    {
          var query = _dbSet.AsQueryable().Where(filter);
          
          if (includeProperties != null)
          {
              foreach (var includeProperty in includeProperties)
              {
                  query = query.Include(includeProperty);
              }
          }

          query.AsNoTracking();
          
          return await query.FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Inserts to dbset
    /// </summary>
    /// <param name="entity">Entity to be insterted</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Entity</returns>
    public virtual async Task<TEntity?> InsertAsync(TEntity? entity, CancellationToken cancellationToken = default)
    {
        var entry = await _dbSet.AddAsync(entity, cancellationToken);
            
        return entry.Entity;
    }

    /// <summary>
    /// Updates entity
    /// </summary>
    /// <param name="entity">Entity to be updated</param>
    public virtual void UpdateAsync(TEntity? entity)
    {
        _dbSet.Attach(entity);
        _dbContext.Entry(entity).State = EntityState.Modified;
    }
}