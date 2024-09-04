using System.Linq.Expressions;
using Plank.Core.Entities;

namespace Plank.Core.Search
{
    public interface ISearchCriteriaBuilder<TEntity> where TEntity : class, IEntity
    {        
        SearchCriteria<TEntity> Build();
        
        ISearchCriteriaBuilder<TEntity> Include(Expression<Func<TEntity, object>> includeExpression);
        
        ISearchCriteriaBuilder<TEntity> SetFilter(Expression<Func<TEntity, bool>> filter, FilterCombination combination);
        
        ISearchCriteriaBuilder<TEntity> SetPageNumber(int pageNumber);
        
        ISearchCriteriaBuilder<TEntity> SetPageSize(int pageSize);
    }
}