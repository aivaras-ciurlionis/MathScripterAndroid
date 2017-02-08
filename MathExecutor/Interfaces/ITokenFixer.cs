using MathExecutor.Models;

namespace MathExecutor.Interfaces
{
    public interface ITokenFixer
    {
        Token GetAditionalToken(Token lastToken, Token currentToken);
    }
}