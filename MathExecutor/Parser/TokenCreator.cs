using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Parser
{
    public class TokenCreator : ITokenCreator
    {
        private readonly IExpressionFactory _expressionFactory;

        public TokenCreator(IExpressionFactory expressionFactory)
        {
            _expressionFactory = expressionFactory;
        }

        public Token GetToken(SymbolType type, string value, int level)
        {

            var token = new Token
            {
                Level = level,
                Value = value
            };

            if (type == SymbolType.Numeric)
            {
                token.Order = 0;
                token.TokenType = TokenType.Number;
                return token;
            }

            var expression = _expressionFactory.GetExpression(value);
            if (expression != null)
            {
                token.Order = expression.Order;
                token.TokenType = TokenType.Operation;
            }
            else
            {
                token.Order = 0;
                token.TokenType = TokenType.Variable;
            }
            return token;
        }
    }
}