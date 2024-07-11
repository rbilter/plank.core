using System.Linq.Expressions;
using X.PagedList;

namespace Plank.Core.Data
{
    public interface IReadRepository<T>
    {
        Task<T> GetAsync(int id);

        Task<IPagedList<T>> SearchAsync(Expression<Func<T, bool>> expression, List<Expression<Func<T, object>>>? includes, int pageNumber, int pageSize);
    }
}