using System.Collections.Generic;
using MathExecutor.Models;

namespace MathExecutor.Interfaces
{
    public interface ITokenParser
    {
        IEnumerable<Token> ParseTokens(string equation);
    }
}