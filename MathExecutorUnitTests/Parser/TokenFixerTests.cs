using MathExecutor.Interfaces;
using MathExecutor.Models;
using MathExecutor.Parser;
using NUnit.Framework;

namespace MathExecutorUnitTests.Parser
{
    [TestFixture]
    public class TokenFixerTests
    {
        private ITokenFixer _tokenFixer;

        [SetUp]
        public void Init()
        {
            _tokenFixer = new TokenFixer();
        }

        [Test]
        public void ItShouldReturnAdditionalTokenIfLastTokenIsNumberAndThisIsParenthesis()
        {
            var lastToken = new Token { TokenType = TokenType.Number, Value = "3" };
            var thisToken = new Token { TokenType = TokenType.Operation, Value = "(" };
            var result = _tokenFixer.GetAditionalToken(lastToken, thisToken);
            Assert.AreEqual("*", result.Value);
        }

        [Test]
        public void ItShouldReturnAdditionalTokenIfLastTokenIsVariableAndThisIsParenthesis()
        {
            var lastToken = new Token { TokenType = TokenType.Variable, Value = "x" };
            var thisToken = new Token { TokenType = TokenType.Operation, Value = "(" };
            var result = _tokenFixer.GetAditionalToken(lastToken, thisToken);
            Assert.AreEqual("*", result.Value);
        }

        [Test]
        public void ItNotShouldReturnAdditionalTokenIfLastTokenIsOperationAndThisIsParenthesis()
        {
            var lastToken = new Token { TokenType = TokenType.Operation, Value = "+" };
            var thisToken = new Token { TokenType = TokenType.Operation, Value = "(" };
            var result = _tokenFixer.GetAditionalToken(lastToken, thisToken);
            Assert.IsNull(result);
        }

        [Test]
        public void ItShouldAddMultiplyTokenBetweenNumberAndVariable()
        {
            var lastToken = new Token { TokenType = TokenType.Number, Value = "2" };
            var thisToken = new Token { TokenType = TokenType.Variable, Value = "x" };
            var result = _tokenFixer.GetAditionalToken(lastToken, thisToken);
            Assert.AreEqual("*", result.Value);
        }

        [Test]
        public void ItShouldNotAddMultiplyTokenBetweeVariableAndNumber()
        {
            var lastToken = new Token { TokenType = TokenType.Variable, Value = "x" };
            var thisToken = new Token { TokenType = TokenType.Number, Value = "6" };
            var result = _tokenFixer.GetAditionalToken(lastToken, thisToken);
            Assert.IsNull(result);
        }

    }
}
