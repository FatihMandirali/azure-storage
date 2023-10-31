using System.Linq.Expressions;

namespace AZ.STORAGE;

public interface INoSqlStorage<TEntity>
{
    Task<TEntity> Add(TEntity entity);
    Task Update(TEntity entity);
    Task<TEntity> Get(string rowKey, string partitionKey);
    Task<IQueryable<TEntity>> All();
    Task Delete(string rowKey, string partitionKey);
    List<TEntity> Query(Expression<Func<TEntity, bool>> expression);
}