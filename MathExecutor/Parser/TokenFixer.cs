using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Parser
{
    public class TokenFixer : ITokenFixer
    {
        public Token GetAditionalToken(Token lastToken, Token currentToken)
        {
            if (lastToken == null || currentToken == null)
            {
                return null;
            }

            if ((lastToken.TokenType == TokenType.Number || lastToken.TokenType == TokenType.Variable)
                && currentToken.Value == "(")
            {
                return Token.MultiplyToken();
            }

            if (lastToken.Value == ")" && currentToken.Value == "(")
            {
                return Token.MultiplyToken();
            }

            if (lastToken.TokenType == TokenType.Number && currentToken.TokenType == TokenType.Variable &&
                currentToken.Value != ")")
            {
                return Token.MultiplyToken();
            }

            return null;
        }
    }
}