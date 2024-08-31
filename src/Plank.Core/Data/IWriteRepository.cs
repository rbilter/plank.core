namespace Plank.Core.Data
{
    public interface IWriteRepository<T>
    {
        Task Add(T entity);

        Task BulkAdd(IEnumerable<T> entities);

        Task Delete(int id);

        Task Update(T entity);
    }
}