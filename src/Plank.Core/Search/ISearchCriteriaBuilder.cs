using System.Linq.Expressions;
using Plank.Core.Entities;

namespace Plank.Core.Search
{
    public interface ISearchCriteriaBuilder<TEntity> where TEntity : class, IEntity
    {
        SearchCriteria<TEntity> Build();

        ISearchCriteriaBuilder<TEntity> AddInclude(Expression<Func<TEntity, object>> includeExpression);

        ISearchCriteriaBuilder<TEntity> AddFilterAnd(Expression<Func<TEntity, bool>> filter);

        ISearchCriteriaBuilder<TEntity> AddFilterOr(Expression<Func<TEntity, bool>> filter);

        ISearchCriteriaBuilder<TEntity> SetPageNumber(int pageNumber);

        ISearchCriteriaBuilder<TEntity> SetPageSize(int pageSize);
    }
}