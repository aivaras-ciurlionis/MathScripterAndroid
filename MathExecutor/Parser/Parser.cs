using MathExecutor.Interfaces;

namespace MathExecutor.Parser
{
    public class Parser : IParser
    {
        private readonly ITokenParser _tokenParser;
        private readonly IExpressionCreator _expressionCreator;

        public Parser(IExpressionCreator expressionCreator, ITokenParser tokenParser)
        {
            _expressionCreator = expressionCreator;
            _tokenParser = tokenParser;
        }

        public IExpression Parse(string equation)
        {
            return _expressionCreator.CreateExpression(_tokenParser.ParseTokens(equation));
        }
    }
}