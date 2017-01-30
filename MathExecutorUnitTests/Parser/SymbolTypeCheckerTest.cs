using MathExecutor.Interfaces;
using MathExecutor.Models;
using MathExecutor.Parser;
using NUnit.Framework;

namespace MathExecutorUnitTests.Parser
{
    [TestFixture]
    public class SymbolTypeCheckerTest
    {
        private ISymbolTypeChecker _typeChecker;

        [SetUp]
        public void Init()
        {
            _typeChecker = new SymbolTypeChecker();
        }

        [Test]
        public void ItShouldReturnSymbolTypeOfNumberIfItIsANumberOrSeparator()
        {
            Assert.AreEqual(SymbolType.Numeric, _typeChecker.GetSymbolType('3'));
            Assert.AreEqual(SymbolType.Numeric, _typeChecker.GetSymbolType('.'));
            Assert.AreEqual(SymbolType.Numeric, _typeChecker.GetSymbolType('0'));
            Assert.AreEqual(SymbolType.Numeric, _typeChecker.GetSymbolType(','));
        }

        [Test]
        public void ItShouldReturnSymbolTypeOfParenthesis()
        {
            Assert.AreEqual(SymbolType.Parenthesis, _typeChecker.GetSymbolType('('));
            Assert.AreEqual(SymbolType.Parenthesis, _typeChecker.GetSymbolType(')'));
        }

        [Test]
        public void ItShouldReturnSymbolTypeOfOther()
        {
            Assert.AreEqual(SymbolType.Symbol, _typeChecker.GetSymbolType('+'));
            Assert.AreEqual(SymbolType.Other, _typeChecker.GetSymbolType('a'));
            Assert.AreEqual(SymbolType.Symbol, _typeChecker.GetSymbolType('='));
            Assert.AreEqual(SymbolType.Symbol, _typeChecker.GetSymbolType('*'));
            Assert.AreEqual(SymbolType.Symbol, _typeChecker.GetSymbolType('!'));
            Assert.AreEqual(SymbolType.Other, _typeChecker.GetSymbolType('☺'));
        }

    }
}
