using MathExecutor.Models;

namespace MathExecutor.Interfaces
{
    public interface ISymbolTypeChecker
    {
        SymbolType GetSymbolType(char symbol);
    }
}