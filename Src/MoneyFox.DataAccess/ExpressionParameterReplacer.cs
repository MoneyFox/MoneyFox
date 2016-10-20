using System.Linq.Expressions;

namespace MoneyFox.DataAccess
{
    public class ParameterReplacer : ExpressionVisitor
    {
        private readonly ParameterExpression _parameter;
        private readonly Expression _replacement;

        private ParameterReplacer(ParameterExpression parameter, Expression replacement)
        {
            _parameter = parameter;
            _replacement = replacement;
        }

        public static Expression Replace(Expression expression, ParameterExpression parameter, Expression replacement)
        {
            return new ParameterReplacer(parameter, replacement).Visit(expression);
        }

        protected override Expression VisitParameter(ParameterExpression parameter)
        {
            if (parameter == _parameter)
            {
                return _replacement;
            }
            return base.VisitParameter(parameter);
        }
    }
}