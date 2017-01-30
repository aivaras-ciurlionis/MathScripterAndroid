using System;
using System.Collections.Generic;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Interfaces;

namespace MathExecutor.Expressions
{
    public class ExpressionFactory : IExpressionFactory
    {
        public IExpression GetExpression(string key)
        {
            var operands = new List<IExpression> { null, null };
            return GetExpression(key, operands);
        }

        public IExpression GetExpression(string key, IList<IExpression> operands)
        {
            switch (key)
            {
                case "+": return new SumExpression(operands[0], operands[1]);
                case "-": return new SubtractExpression(operands[0], operands[1]);
                case "*": return new MultiplyExpression(operands[0], operands[1]);
                case "(": return new ParenthesisExpression(operands[0]);
                default:
                    return null;
            }
        }
    }
}