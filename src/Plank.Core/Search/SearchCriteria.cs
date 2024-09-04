using System.Linq.Expressions;
using Plank.Core.Entities;

namespace Plank.Core.Search
{
    public class SearchCriteria<TEntity> where TEntity : class, IEntity
    {
        public Expression<Func<TEntity, bool>> Filter { get; set; } = e => true;

        public List<Expression<Func<TEntity, object>>> Includes { get; set; } = [];

        public int PageNumber { get; set; }
        
        public int PageSize { get; set; }
    }
}