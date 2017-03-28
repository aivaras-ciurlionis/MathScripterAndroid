using System.Collections.Generic;
using System.Linq;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Helpers
{
    public class ExpressionFlatener : IExpressionFlatener
    {
        private int _number = 0;

        public IEnumerable<FlatExpressionResult> FlattenExpression(IExpression expression,
            bool onlyFirstLevel = false, 
            bool includeMonomial = false)
        {
            _number = 0;
            var expressions = FlattenRecursive(expression, 0);
            if (!includeMonomial)
            {
                expressions = expressions.Where(e => !(e.Expression is Monomial));
            }
            if (onlyFirstLevel)
            {
                expressions = expressions.Where(e => e.Level < 1);
            }
            return expressions;
        }

        private IEnumerable<FlatExpressionResult> FlattenRecursive(IExpression expression, int level)
        {
            var expressions = new List<FlatExpressionResult>();

            if (IsDeeperLevel(expression))
            {
                level += 1;
                _number += 1;
            }

            foreach (var operand in expression.Operands ?? new List<IExpression>())
            {
                expressions.AddRange(FlattenRecursive(operand, level));
            }
            expressions.Add(new FlatExpressionResult
            {
                Expression = expression,
                Level = level,
                Number = _number
            });
            return expressions;
        }

        private static bool IsDeeperLevel(IExpression expression)
        {
            return !(expression is SumExpression || 
                expression is SubtractExpression || 
                expression is MultiplyExpression || 
                expression is Monomial);
        }

    }
}