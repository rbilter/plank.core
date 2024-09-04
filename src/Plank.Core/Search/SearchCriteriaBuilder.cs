using System.Linq.Expressions;
using Plank.Core.Entities;

namespace Plank.Core.Search
{
    public class SearchCriteriaBuilder<TEntity> : ISearchCriteriaBuilder<TEntity> where TEntity : class, IEntity
    {
        private readonly SearchCriteria<TEntity> _criteria = new();

        public SearchCriteria<TEntity> Build()
        {
            return _criteria;
        }

        public ISearchCriteriaBuilder<TEntity> Include(Expression<Func<TEntity, object>> includeExpression)
        {
            _criteria.Includes.Add(includeExpression);
            return this;
        }

        public ISearchCriteriaBuilder<TEntity> SetFilter(Expression<Func<TEntity, bool>> filter, FilterCombination combination)
        {
            _criteria.Filter = combination switch
            {
                FilterCombination.And => _criteria.Filter.And(filter),
                FilterCombination.Or => _criteria.Filter.Or(filter),
                _ => throw new ArgumentOutOfRangeException(nameof(combination), combination, null)
            };
            return this;
        }

        public ISearchCriteriaBuilder<TEntity> SetPageNumber(int pageNumber)
        {
            _criteria.PageNumber = pageNumber;
            return this;
        }

        public ISearchCriteriaBuilder<TEntity> SetPageSize(int pageSize)
        {
            _criteria.PageSize = pageSize;
            return this;
        }
    }
}