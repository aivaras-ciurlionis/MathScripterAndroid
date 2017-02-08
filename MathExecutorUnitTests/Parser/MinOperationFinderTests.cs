using System.Collections.Generic;
using MathExecutor.Interfaces;
using MathExecutor.Models;
using MathExecutor.Parser;
using NUnit.Framework;

namespace MathExecutorUnitTests.Parser
{
    [TestFixture]
    public class MinOperationFinderTests
    {
        private IMinOperationFinder _minOperationFinder;

        [SetUp]
        public void Init()
        {
            _minOperationFinder = new MinOperationFinder();

        }

        [Test]
        public void ShouldReturnFirstOperationIndex()
        {
            var tokens = new List<Token>
            {
                new Token {Level = 0, Order = 0, Value = "2", TokenType = TokenType.Number},
                new Token {Level = 0, Order = 2, Value = "+", TokenType = TokenType.Operation},
                new Token {Level = 0, Order = 0, Value = "8", TokenType = TokenType.Number},
                new Token {Level = 0, Order = 1, Value = "*", TokenType = TokenType.Operation},
                new Token {Level = 0, Order = 2, Value = "4", TokenType = TokenType.Number}
            };
            Assert.AreEqual(1, _minOperationFinder.FindMinOperationIndex(tokens));
        }

        [Test]
        public void ShouldReturnLastOperationIndex()
        {
            var tokens = new List<Token>
            {
                new Token {Level = 0, Order = 0, Value = "2", TokenType = TokenType.Number},
                new Token {Level = 0, Order = 1, Value = "*", TokenType = TokenType.Operation},
                new Token {Level = 0, Order = 0, Value = "8", TokenType = TokenType.Number},
                new Token {Level = 0, Order = 2, Value = "-", TokenType = TokenType.Operation},
                new Token {Level = 0, Order = 2, Value = "4", TokenType = TokenType.Number}
            };
            Assert.AreEqual(3, _minOperationFinder.FindMinOperationIndex(tokens));
        }

        [Test]
        public void ShouldReturnOperationIndexWithParenthesis()
        {
            var tokens = new List<Token>
            {
                new Token {Level = 1, Order = 5, Value = "(", TokenType = TokenType.Operation},
                new Token {Level = 1, Order = 0, Value = "2", TokenType = TokenType.Number},
                new Token {Level = 1, Order = 1, Value = "*", TokenType = TokenType.Operation},
                new Token {Level = 1, Order = 0, Value = "8", TokenType = TokenType.Number},
                new Token {Level = 0, Order = 2, Value = "+", TokenType = TokenType.Operation},
                new Token {Level = 0, Order = 0, Value = "4", TokenType = TokenType.Number}
            };
            Assert.AreEqual(4, _minOperationFinder.FindMinOperationIndex(tokens));
        }

        [Test]
        public void ShouldReturnOperationIndexInParenthesis()
        {
            var tokens = new List<Token>
            {
                new Token {Level = 1, Order = 5, Value = "(", TokenType = TokenType.Operation},
                new Token {Level = 1, Order = 0, Value = "2", TokenType = TokenType.Number},
                new Token {Level = 1, Order = 2, Value = "-", TokenType = TokenType.Operation},
                new Token {Level = 1, Order = 0, Value = "8", TokenType = TokenType.Number}
            };
            Assert.AreEqual(0, _minOperationFinder.FindMinOperationIndex(tokens));
        }

    }
}
