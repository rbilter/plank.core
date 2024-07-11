using System.Linq.Expressions;
using Plank.Core.Contracts;

namespace Plank.Core.Services
{
    public interface IWriteService<T> where T : new()
    {
        Task<PlankPostResponse<T>> AddAsync(T item);

        Task<PlankBulkPostResponse<T>> BulkAddAsync(IEnumerable<T> items);

        Task<PlankDeleteResponse> DeleteAsync(int id);

        Task<PlankPostResponse<T>> UpdateAsync(T item);

        Task<PlankPostResponse<T>> UpdateAsync(T item, params Expression<Func<T, object>>[] properties);
    }
}