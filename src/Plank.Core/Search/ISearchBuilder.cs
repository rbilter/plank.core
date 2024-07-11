using System.Linq.Expressions;
using Plank.Core.Models;

namespace Plank.Core.Search
{
    public interface ISearchBuilder<TEntity> where TEntity : class, IEntity
    {
        List<Expression<Func<TEntity, object>>> Includes { get; }

        int PageNumber { get; }

        int PageSize { get; }

        Expression<Func<TEntity, bool>> Build();
    }
}