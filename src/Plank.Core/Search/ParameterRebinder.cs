using System.Linq.Expressions;

namespace Plank.Core.Search
{ 
    internal class ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map) : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> _map = map;

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            if (_map.TryGetValue(p, out ParameterExpression? replacement))
            {
                p = replacement;
            }
            return base.VisitParameter(p);
        }
    }
}