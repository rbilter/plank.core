using System.Linq.Expressions;
using Plank.Core.Entities;

namespace Plank.Core.Search
{
    public static class ExtensionMethods
    {
        public static Expression<Func<TEntity, bool>> And<TEntity>(this Expression<Func<TEntity, bool>> first, Expression<Func<TEntity, bool>> second) where TEntity : IEntity
        {
            _ = first ?? throw new ArgumentNullException(nameof(first));
            _ = second ?? throw new ArgumentNullException(nameof(second));
            
            return first.Compose(second, Expression.And);
        }

        public static Expression<Func<TEntity, bool>> Or<TEntity>(this Expression<Func<TEntity, bool>> first, Expression<Func<TEntity, bool>> second) where TEntity : IEntity
        {
            _ = first ?? throw new ArgumentNullException(nameof(first));
            _ = second ?? throw new ArgumentNullException(nameof(second));
                        
            return first.Compose(second, Expression.Or);
        }

        private static Expression<TEntity> Compose<TEntity>(this Expression<TEntity> first, Expression<TEntity> second, Func<Expression, Expression, Expression> merge)
        {
            var map        = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            return Expression.Lambda<TEntity>(merge(first.Body, secondBody), first.Parameters);
        }
    }
}