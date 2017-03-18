using MathExecutor.Expressions;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Interfaces;
using MathExecutor.Parser;
using NUnit.Framework;

namespace MathExecutorUnitTests.ModuleIntegrationTests
{
    [TestFixture]
    public class ParserTests
    {
        private IParser _parser;

        [SetUp]
        public void Init()
        {
            var monomialResolver = new MonomialResolver();
            var expressionFactory = new ExpressionFactory();
            var minOperationFinder = new MinOperationFinder();
            var expressionCreator = new ExpressionCreator(expressionFactory, monomialResolver, minOperationFinder);
            var symbolTypeChecker = new SymbolTypeChecker();
            var tokenCreator = new TokenCreator(expressionFactory);
            var tokenFixer = new TokenFixer();
            var tokenParser = new TokenParser(symbolTypeChecker, tokenCreator, tokenFixer);
            _parser = new MathExecutor.Parser.Parser(expressionCreator, tokenParser);
        }

        [Test]
        public void ItShouldParseSingleExpression()
        {
            var result = _parser.Parse("2+2");
            Assert.AreEqual("2 + 2", result.ToString());
        }

        [Test]
        public void ItShouldParseDoubleExpression()
        {
            var result = _parser.Parse("2+2*4");
            Assert.AreEqual("2 + 2 * 4", result.ToString());
        }

        [Test]
        public void ItShouldParseLongExpressionWithFraction()
        {
            var result = _parser.Parse("2+2*4-6*12.3");
            Assert.AreEqual("2 + 2 * 4 - 6 * 12.3", result.ToString());
        }

        [Test]
        public void ItShouldParseLongExpressionWithParenthesis()
        {
            var result = _parser.Parse("(2+2*4)-(6*12.3)");
            Assert.AreEqual("(2 + 2 * 4) - (6 * 12.3)", result.ToString());
        }

        [Test]
        public void ItShouldParseLongExpressionWithMultiLevel()
        {
            var result = _parser.Parse("(1-(2-(4*12.6)))");
            Assert.AreEqual("(1 - (2 - (4 * 12.6)))", result.ToString());
            Assert.AreEqual(typeof(ParenthesisExpression), result.GetType());
        }

        [Test]
        public void ItShouldIgnoreSpacesInExpression()
        {
            var result = _parser.Parse("2 + 3");
            Assert.AreEqual("2 + 3", result.ToString());
            Assert.AreEqual(typeof(SumExpression), result.GetType());
        }

        [Test]
        public void ItShouldIgnoreMultipleSpacesInExpression()
        {
            var result = _parser.Parse("2    -  2   .   9");
            Assert.AreEqual("2 - 2.9", result.ToString());
            Assert.AreEqual(typeof(SubtractExpression), result.GetType());
        }

        [Test]
        public void ItShouldParseStringsWithUnarySubtraction()
        {
            var result = _parser.Parse("-14-6.1");
            Assert.AreEqual("-14 - 6.1", result.ToString());
            Assert.AreEqual(typeof(SubtractExpression), result.GetType());
        }

        [Test]
        public void ItShouldParseStringsWithUnarySubtractionInsideParenthesis()
        {
            var result = _parser.Parse("2*(-3+(-6))");
            Assert.AreEqual("2 * (-3 + (-6))", result.ToString());
            Assert.AreEqual(typeof(MultiplyExpression), result.GetType());
        }

        [Test]
        public void ItShouldNegateExpressionInParenthesis()
        {
            var result = _parser.Parse("-(1+2+3*4)");
            Assert.AreEqual("-(1 + 2 + 3 * 4)", result.ToString());
            Assert.AreEqual(typeof(NegationExpression), result.GetType());
        }

        [Test]
        public void ItShouldImplicitlyAddMultiplicationBeforeParenthesisForNumbers()
        {
            var result = _parser.Parse("2(3+y)");
            Assert.AreEqual("2 * (3 + y)", result.ToString());
            Assert.AreEqual(typeof(MultiplyExpression), result.GetType());
        }

        [Test]
        public void ItShouldImplicitlyAddMultiplicationBeforeParenthesisForVariables()
        {
            var result = _parser.Parse("2+x(4-1)");
            Assert.AreEqual("2 + x * (4 - 1)", result.ToString());
            Assert.AreEqual(typeof(SumExpression), result.GetType());
        }

        [Test]
        public void ItShouldImplicitlyAddMultiplicationBetweenVariableAndNumber()
        {
            var result = _parser.Parse("2x+3y");
            Assert.AreEqual("2 * x + 3 * y", result.ToString());
            Assert.AreEqual(typeof(SumExpression), result.GetType());
        }

        [Test]
        public void ItShouldImplicitlyAddMultiplicationBetweenMultiVariableAndNumber()
        {
            var result = _parser.Parse("2xy-3xyz");
            Assert.AreEqual("2 * xy - 3 * xyz", result.ToString());
            Assert.AreEqual(typeof(SubtractExpression), result.GetType());
        }

        [Test]
        public void ItShouldImplicitlyAddMultiplicationInDivision()
        {
            var result = _parser.Parse("(3x^2+4x-3)/(9x^2+3x+4)");
            Assert.AreEqual("(3 * x ^ 2 + 4 * x - 3) / (9 * x ^ 2 + 3 * x + 4)", result.ToString());
            Assert.AreEqual(typeof(DivisionExpression), result.GetType());
        }

    }
}
