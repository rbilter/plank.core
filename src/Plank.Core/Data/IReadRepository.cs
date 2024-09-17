using System.Linq.Expressions;
using X.PagedList;

namespace Plank.Core.Data
{
    public interface IReadRepository<TEntity>
    {
        Task<TEntity> Get(int id);

        Task<IPagedList<TEntity>> Search(Expression<Func<TEntity, bool>> filter, List<Expression<Func<TEntity, object>>>? includes, int pageNumber, int pageSize);
    }
}