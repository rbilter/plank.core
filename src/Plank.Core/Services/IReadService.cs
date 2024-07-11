using System.Linq.Expressions;
using Plank.Core.Contracts;

namespace Plank.Core.Services
{
    public interface IReadService<T> where T : new()
    {
        Task<PlankGetResponse<T>> GetAsync(int id);

        Task<PlankEnumerableResponse<T>> SearchAsync(Expression<Func<T, bool>> expression, List<Expression<Func<T, object>>> includes, int pageNumber, int pageSize);
    }
}