using System.Linq.Expressions;
using X.PagedList;

namespace Plank.Core.Data
{
    public interface IReadRepository<T>
    {
        Task<T> Get(int id);

        Task<IPagedList<T>> Search(Expression<Func<T, bool>> filter, List<Expression<Func<T, object>>>? includes, int pageNumber, int pageSize);
    }
}