using MathExecutor.Models;

namespace MathExecutor.Interfaces
{
    public interface IMonomialResolver
    {
        Monomial GetMonomial(Token token);
    }
}