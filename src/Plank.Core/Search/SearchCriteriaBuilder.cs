using System;
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

        public ISearchCriteriaBuilder<TEntity> AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            _criteria.Includes.Add(includeExpression);
            return this;
        }

        public ISearchCriteriaBuilder<TEntity> AddFilterAnd(Expression<Func<TEntity, bool>> filter)
        {
            _criteria.Filter = _criteria.Filter.And(filter);
            return this;
        }

        public ISearchCriteriaBuilder<TEntity> AddFilterOr(Expression<Func<TEntity, bool>> filter)
        {
            _criteria.Filter = _criteria.Filter.Or(filter);
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