using System;
using System.Linq;
using MathExecutor.Interfaces;
using MathExecutor.Models;
using MathExecutor.Parser;
using NSubstitute;
using NSubstitute.Core.Arguments;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;

namespace MathExecutorUnitTests.Parser
{
    [TestFixture]
    public class TokenParserTest
    {
        private ITokenParser _tokenParser;
        private ISymbolTypeChecker _symbolTypeChecker;
        private ITokenCreator _tokenCreator;
        private ITokenFixer _tokenFixer;

        [SetUp]
        public void Init()
        {
            _symbolTypeChecker = Substitute.For<ISymbolTypeChecker>();
            _tokenCreator = Substitute.For<ITokenCreator>();
            _tokenFixer = Substitute.For<ITokenFixer>();

            _tokenFixer.GetAditionalToken(Arg.Any<Token>(), Arg.Any<Token>()).ReturnsNullForAnyArgs();

            _symbolTypeChecker.GetSymbolType('2').Returns(SymbolType.Numeric);
            _symbolTypeChecker.GetSymbolType('3').Returns(SymbolType.Numeric);
            _symbolTypeChecker.GetSymbolType('.').Returns(SymbolType.Numeric);

            _symbolTypeChecker.GetSymbolType('+').Returns(SymbolType.Symbol);
            _symbolTypeChecker.GetSymbolType('*').Returns(SymbolType.Symbol);
            _symbolTypeChecker.GetSymbolType('(').Returns(SymbolType.Parenthesis);
            _symbolTypeChecker.GetSymbolType(')').Returns(SymbolType.Parenthesis);
            _symbolTypeChecker.GetSymbolType('a').Returns(SymbolType.Other);

            _tokenCreator.GetToken(SymbolType.Numeric, "2", 0, Arg.Any<Token>())
                .Returns(new Token { Level = 0, Order = 0, TokenType = TokenType.Number, Value = "2" });

            _tokenCreator.GetToken(SymbolType.Numeric, "2", 1, Arg.Any<Token>())
                .Returns(new Token { Level = 1, Order = 0, TokenType = TokenType.Number, Value = "2" });

            _tokenCreator.GetToken(SymbolType.Numeric, "223", 1, Arg.Any<Token>())
               .Returns(new Token { Level = 1, Order = 0, TokenType = TokenType.Number, Value = "223" });

            _tokenCreator.GetToken(SymbolType.Numeric, "3", 0, Arg.Any<Token>())
               .Returns(new Token { Level = 0, Order = 0, TokenType = TokenType.Number, Value = "3" });

            _tokenCreator.GetToken(SymbolType.Numeric, "23", 0, Arg.Any<Token>())
               .Returns(new Token { Level = 0, Order = 0, TokenType = TokenType.Number, Value = "23" });

            _tokenCreator.GetToken(SymbolType.Numeric, "2.33", 0, Arg.Any<Token>())
                .Returns(new Token { Level = 0, Order = 0, TokenType = TokenType.Number, Value = "2.33" });

            _tokenCreator.GetToken(SymbolType.Numeric, "3.23", 0, Arg.Any<Token>())
               .Returns(new Token { Level = 0, Order = 0, TokenType = TokenType.Number, Value = "3.23" });

            _tokenCreator.GetToken(SymbolType.Numeric, "3", 2, Arg.Any<Token>())
               .Returns(new Token { Level = 2, Order = 0, TokenType = TokenType.Number, Value = "3" });

            _tokenCreator.GetToken(SymbolType.Symbol, "+", 0, Arg.Any<Token>())
               .Returns(new Token { Level = 0, Order = 2, TokenType = TokenType.Operation, Value = "+" });

            _tokenCreator.GetToken(SymbolType.Parenthesis, "(", 1, Arg.Any<Token>())
              .Returns(new Token { Level = 1, Order = 3, TokenType = TokenType.Operation, Value = "(" });

            _tokenCreator.GetToken(SymbolType.Parenthesis, "(", 2, Arg.Any<Token>())
              .Returns(new Token { Level = 2, Order = 3, TokenType = TokenType.Operation, Value = "(" });

            _tokenCreator.GetToken(SymbolType.Symbol, "+", 1, Arg.Any<Token>())
               .Returns(new Token { Level = 0, Order = 2, TokenType = TokenType.Operation, Value = "+" });

            _tokenCreator.GetToken(SymbolType.Symbol, "*", 0, Arg.Any<Token>())
               .Returns(new Token { Level = 0, Order = 1, TokenType = TokenType.Operation, Value = "*" });

            _tokenCreator.GetToken(SymbolType.Symbol, "*", 1, Arg.Any<Token>())
               .Returns(new Token { Level = 1, Order = 1, TokenType = TokenType.Operation, Value = "*" });

            _tokenCreator.GetToken(SymbolType.Symbol, "*", 2, Arg.Any<Token>())
               .Returns(new Token { Level = 2, Order = 1, TokenType = TokenType.Operation, Value = "*" });

            _tokenCreator.GetToken(SymbolType.Other, "a", 0, Arg.Any<Token>())
               .Returns(new Token { Level = 0, Order = 0, TokenType = TokenType.Variable, Value = "a" });

            _tokenParser = new TokenParser(_symbolTypeChecker, _tokenCreator, _tokenFixer);
        }

        [Test]
        public void ItShouldParseStringWithNoParenthesis()
        {
            var tokens = _tokenParser.ParseTokens("2+3*23+a*2.33").ToList();
            Assert.AreEqual(9, tokens.Count);
            Assert.AreEqual(TokenType.Number, tokens.First().TokenType);
            Assert.AreEqual("2", tokens.First().Value);
            Assert.AreEqual("2.33", tokens.Last().Value);
            Assert.AreEqual(TokenType.Variable, tokens[6].TokenType);
            Assert.AreEqual(TokenType.Operation, tokens[1].TokenType);
        }

        [Test]
        public void ItShouldParseStringWithSingleParenthesis()
        {
            var tokens = _tokenParser.ParseTokens("2+3.23*(2+223)").ToList();
            Assert.AreEqual(8, tokens.Count);
            Assert.AreEqual("223", tokens.Last().Value);
            Assert.AreEqual(TokenType.Operation, tokens[4].TokenType);
        }

        [Test]
        public void ItShouldParseStringWithTwoSingleParenthesis()
        {
            var tokens = _tokenParser.ParseTokens("(223*2)+2+3.23*(2+223)").ToList();
            Assert.AreEqual(13, tokens.Count);
            Assert.AreEqual("(", tokens.First().Value);
            Assert.AreEqual(1, tokens[1].Level);
            Assert.AreEqual("223", tokens.Last().Value);
            Assert.AreEqual(TokenType.Operation, tokens[4].TokenType);
            Assert.AreEqual("+", tokens[4].Value);
            Assert.AreEqual(0, tokens[4].Level);
        }

        [Test]
        public void ItShouldParseStringWithMultiParenthesis()
        {
            var tokens = _tokenParser.ParseTokens("((3*3))+2").ToList();
            Assert.AreEqual(7, tokens.Count);
            Assert.AreEqual("(", tokens.First().Value);
            Assert.AreEqual("(", tokens[1].Value);
            Assert.AreEqual("3", tokens[2].Value);
            Assert.AreEqual(2, tokens[2].Level);
        }

        [Test]
        public void ItShouldThrowExceptionForUnequalParenthesis()
        {
            Assert.Throws<ArgumentException>(() => _tokenParser.ParseTokens("(2+3*2))"));
        }

    }
}
