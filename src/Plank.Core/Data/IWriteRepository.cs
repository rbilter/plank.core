namespace Plank.Core.Data
{
    public interface IWriteRepository<TEntity>
    {
        Task Add(TEntity entity);

        Task BulkAdd(IEnumerable<TEntity> entities);

        Task Delete(int id);

        Task Update(TEntity entity);
    }
}