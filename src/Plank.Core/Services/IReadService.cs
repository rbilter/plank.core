using System.Linq.Expressions;
using Plank.Core.Contracts;
using Plank.Core.Entities;

namespace Plank.Core.Services
{
    public interface IReadService<TEntity> where TEntity : IEntity, new()
    {
        Task<PlankGetResponse<TEntity>> Get(int id);

        Task<PlankEnumerableResponse<TEntity>> Search(Expression<Func<TEntity, bool>> filter, List<Expression<Func<TEntity, object>>> includes, int pageNumber, int pageSize);
    }
}