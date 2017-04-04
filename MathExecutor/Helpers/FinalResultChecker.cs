using System;
using System.Linq;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Helpers
{
    public class FinalResultChecker : IFinalResultChecker
    {
        private readonly IExpressionFlatener _expressionFlatener;

        public FinalResultChecker(IExpressionFlatener expressionFlatener)
        {
            _expressionFlatener = expressionFlatener;
        }

        public bool IsFinalResult(IExpression expression)
        {
            var monomial = expression as Monomial;
            if (monomial != null)
            {
                return true;
            }
            var expressions = _expressionFlatener.FlattenExpression(expression);
            var equations = expressions.Where(e => e.Expression.Type == ExpressionType.Equation);
            return equations.All(
                e => 
                e.Expression != null &&
                e.Expression.Operands.Count == 2 &&
                e.Expression.Operands[0] is Monomial &&
                e.Expression.Operands[1] is Monomial &&
                IsSingleVariable(e.Expression.Operands[0]) &&
                IsNumericMonomial(e.Expression.Operands[1])
            );
        }

        public bool IsSingleVariable(IExpression m)
        {
            var monomial = m as Monomial;
            return monomial != null &&
                   Math.Abs(monomial.Coefficient - 1) < 0.001 &&
                   monomial.Variables.Count() == 1 &&
                   Math.Abs(monomial.Variables.First().Exponent - 1) < 0.001;
        }

        public bool IsNumericMonomial(IExpression m)
        {
            var monomial = m as Monomial;
            return monomial != null && monomial.IsNumeral();
        }

    }
}