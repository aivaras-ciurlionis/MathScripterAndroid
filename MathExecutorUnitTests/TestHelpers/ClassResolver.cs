using MathExecutor.Expressions;
using MathExecutor.Interfaces;
using MathExecutor.Parser;

namespace MathExecutorUnitTests.TestHelpers
{
    public class ClassResolver
    {
        public static IParser GetParser()
        {
            var monomialResolver = new MonomialResolver();
            var expressionFactory = new ExpressionFactory();
            var minOperationFinder = new MinOperationFinder();
            var expressionCreator = new ExpressionCreator(expressionFactory, monomialResolver, minOperationFinder);
            var symbolTypeChecker = new SymbolTypeChecker();
            var tokenCreator = new TokenCreator(expressionFactory);
            var tokenFixer = new TokenFixer();
            var tokenParser = new TokenParser(symbolTypeChecker, tokenCreator, tokenFixer);
            return new MathExecutor.Parser.Parser(expressionCreator, tokenParser);
        }
    }
}
