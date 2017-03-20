using System.Collections.Generic;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Expressions.Equality;
using MathExecutor.Expressions.Trigonomethric;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Expressions
{
    public class ExpressionFactory : IExpressionFactory
    {
        public IExpression GetExpression(string key)
        {
            var operands = new List<IExpression> { null, null };
            return GetExpression(key, operands);
        }

        private static IExpression GetSubtractExpression(IExpression op1,
            IExpression op2,
            TokenType lastTokenType)
        {
            if (lastTokenType == TokenType.Number || lastTokenType == TokenType.Variable)
            {
                return new SubtractExpression(op1, op2);
            }
            return new NegationExpression(op1);
        }

        public IExpression GetExpression(string key, IList<IExpression> operands)
        {
            return GetExpression(key, operands, operands.Count == 1 ? TokenType.Other : TokenType.Number);
        }

        public IExpression GetExpression(string key, Token lastToken)
        {
            var operands = new List<IExpression> { null, null };
            return GetExpression(key, operands, lastToken?.TokenType ?? TokenType.Other);
        }

        private IExpression GetExpression(string key, IList<IExpression> operands, TokenType lastTokenType)
        {
            switch (key)
            {
                case "+": return new SumExpression(operands[0], operands[1]);
                case "-": return GetSubtractExpression(operands[0], operands.Count > 1 ? operands[1] : null, lastTokenType);
                case "*": return new MultiplyExpression(operands[0], operands[1]);
                case "(": return new ParenthesisExpression(operands[0]);
                case "^": return new ExponentExpression(operands[0], operands[1]);
                case "/": return new DivisionExpression(operands[0], operands[1]);
                case "=": return new EqualityExpression(operands[0], operands[1]);
                case "sin": return new SinExpression(operands[0]);
                case "sqrt": return new SqrRootExpression(operands[0]);
                default:
                    return null;
            }
        }
    }
}