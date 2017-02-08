using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Interfaces;
using MathExecutor.Models;
using MathExecutor.Parser;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;

namespace MathExecutorUnitTests.Parser
{
    [TestFixture]
    public class TokenCreatorTest
    {
        private IExpressionFactory _expressionFactory;
        private ITokenCreator _tokenCreator;

        [SetUp]
        public void Init()
        {
            _expressionFactory = Substitute.For<IExpressionFactory>();
            _expressionFactory.GetExpression("a", Arg.Any<Token>()).ReturnsNull();
            _expressionFactory.GetExpression("x", Arg.Any<Token>()).ReturnsNull();
            _expressionFactory.GetExpression("abc", Arg.Any<Token>()).ReturnsNull();
            _expressionFactory.GetExpression("+", Arg.Any<Token>()).Returns(new SumExpression(null, null));
            _expressionFactory.GetExpression("*", Arg.Any<Token>()).Returns(new MultiplyExpression(null, null));
            _tokenCreator = new TokenCreator(_expressionFactory);
        }

        [Test]
        public void ShouldReturnNumericToken()
        {
            var createdToken = _tokenCreator.GetToken(SymbolType.Numeric, "1234.5", 1);
            Assert.AreEqual(TokenType.Number, createdToken.TokenType);
            Assert.AreEqual("1234.5", createdToken.Value);
            Assert.AreEqual(1, createdToken.Level);
            Assert.AreEqual(0, createdToken.Order);
        }

        [Test]
        public void ShouldReturnOperationToken()
        {
            var createdToken = _tokenCreator.GetToken(SymbolType.Other, "+", 1);
            Assert.AreEqual(TokenType.Operation, createdToken.TokenType);
            Assert.AreEqual("+", createdToken.Value);
            Assert.AreEqual(1, createdToken.Level);
            Assert.AreEqual(3, createdToken.Order);
        }

        [Test]
        public void ShouldReturnVariableToken()
        {
            var createdToken = _tokenCreator.GetToken(SymbolType.Other, "x", 3);
            Assert.AreEqual(TokenType.Variable, createdToken.TokenType);
            Assert.AreEqual("x", createdToken.Value);
            Assert.AreEqual(3, createdToken.Level);
            Assert.AreEqual(0, createdToken.Order);
        }

    }
}
