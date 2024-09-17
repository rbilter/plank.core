using System.Linq.Expressions;
using Plank.Core.Contracts;
using Plank.Core.Entities;

namespace Plank.Core.Services
{
    public interface IWriteService<TEntity> where TEntity : IEntity, new()
    {
        Task<PlankPostResponse<TEntity>> Add(TEntity item);

        Task<PlankBulkPostResponse<TEntity>> BulkAdd(IEnumerable<TEntity> items);

        Task<PlankDeleteResponse> Delete(int id);

        Task<PlankPostResponse<TEntity>> Update(TEntity item);

        Task<PlankPostResponse<TEntity>> Update(TEntity item, params Expression<Func<TEntity, object>>[] properties);
    }
}