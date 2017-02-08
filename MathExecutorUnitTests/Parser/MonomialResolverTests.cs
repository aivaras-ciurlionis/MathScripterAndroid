using System;
using System.Linq;
using MathExecutor.Interfaces;
using MathExecutor.Models;
using MathExecutor.Parser;
using NUnit.Framework;

namespace MathExecutorUnitTests.Parser
{
    [TestFixture]
    public class MonomialResolverTests
    {
        private IMonomialResolver _monomialResolver;

        [SetUp]
        public void Init()
        {
            _monomialResolver = new MonomialResolver(); ;
        }

        [Test]
        public void ShouldThrowExceptionIfTokenIsNull()
        {
            Assert.Throws<ArgumentException>(() => _monomialResolver.GetMonomial(null));
        }

        [Test]
        public void ShouldThrowExceptionIfTokenIsWronkType()
        {
            var token = new Token { TokenType = TokenType.Operation };
            Assert.Throws<ArgumentException>(() => _monomialResolver.GetMonomial(token));
            token = new Token { TokenType = TokenType.Other };
            Assert.Throws<ArgumentException>(() => _monomialResolver.GetMonomial(token));
        }

        [Test]
        public void ShouldThrowExceptionIfDoubleCannotBeParsed()
        {
            var token = new Token { TokenType = TokenType.Number, Value = "ew"};
            Assert.Throws<FormatException>(() => _monomialResolver.GetMonomial(token));
        }

        [Test]
        public void ShouldThrowExceptionIfThereAreVariablesWithSameName()
        {
            var token = new Token { TokenType = TokenType.Variable, Value = "xyipyx" };
            Assert.Throws<ArgumentException>(() => _monomialResolver.GetMonomial(token));
        }

        [Test]
        public void ShouldParseDoubleIfTokenTypeIsNumber()
        {
            var token = new Token {TokenType = TokenType.Number, Value = "7.03"};
            var result = _monomialResolver.GetMonomial(token);
            Assert.AreEqual(7.03, result.Coefficient, 0.01);
        }

        [Test]
        public void ShouldParseDoubleIfTokenIsNegativeNumber()
        {
            var token = new Token { TokenType = TokenType.Number, Value = "-8" };
            var result = _monomialResolver.GetMonomial(token);
            Assert.AreEqual(-8, result.Coefficient, 0.01);
        }

        [Test]
        public void ShouldParseDoubleIfTokenIsSingleVariable()
        {
            var token = new Token { TokenType = TokenType.Variable, Value = "x" };
            var result = _monomialResolver.GetMonomial(token);
            Assert.AreEqual(1, result.Coefficient, 0.01);
            Assert.AreEqual(1, result.Variables.Count());
            Assert.AreEqual("x", result.Variables.ElementAt(0).Name);
        }

        [Test]
        public void ShouldParseDoubleIfTokenHasMultiVariables()
        {
            var token = new Token { TokenType = TokenType.Variable, Value = "xyz" };
            var result = _monomialResolver.GetMonomial(token);
            Assert.AreEqual(1, result.Coefficient, 0.01);
            Assert.AreEqual(3, result.Variables.Count());
            Assert.AreEqual("x", result.Variables.ElementAt(0).Name);
            Assert.AreEqual("y", result.Variables.ElementAt(1).Name);
            Assert.AreEqual("z", result.Variables.ElementAt(2).Name);
        }

    }
}
