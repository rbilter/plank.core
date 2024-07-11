namespace Plank.Core.Data
{
    public interface IWriteRepository<T>
    {
        Task AddAsync(T entity);

        Task BulkAddAsync(IEnumerable<T> entities);

        Task DeleteAsync(int id);

        Task UpdateAsync(T entity);
    }
}