using System.Linq.Expressions;
using Plank.Core.Contracts;
using Plank.Core.Entities;

namespace Plank.Core.Services
{
    public interface IReadService<T> where T : IEntity, new()
    {
        Task<PlankGetResponse<T>> Get(int id);

        Task<PlankEnumerableResponse<T>> Search(Expression<Func<T, bool>> expression, List<Expression<Func<T, object>>> includes, int pageNumber, int pageSize);
    }
}