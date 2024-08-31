using System.Linq.Expressions;
using Plank.Core.Contracts;

namespace Plank.Core.Services
{
    public interface IWriteService<T> where T : new()
    {
        Task<PlankPostResponse<T>> Add(T item);

        Task<PlankBulkPostResponse<T>> BulkAdd(IEnumerable<T> items);

        Task<PlankDeleteResponse> Delete(int id);

        Task<PlankPostResponse<T>> Update(T item);

        Task<PlankPostResponse<T>> Update(T item, params Expression<Func<T, object>>[] properties);
    }
}