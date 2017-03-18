using MathExecutor.Expressions;
using MathExecutor.Interfaces;
using MathExecutor.Interpreter;
using MathExecutor.Parser;
using NUnit.Framework;

namespace MathExecutorUnitTests.ModuleIntegrationTests
{
    [TestFixture]
    public class NumeralArithmeticIntegrationTests
    {
        private IParser _parser;
        private IInterpreter _interpreter;

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
            _interpreter = new Interpreter(_parser);
        }

        [Test]
        public void ItShoutComuputeSingleExpression()
        {
            Assert.AreEqual(4, _interpreter.FindSolution("2+2").NumericResult);
        }

        [Test]
        public void ItShoutComuputeSingleExpression2()
        {
            Assert.AreEqual(10, _interpreter.FindSolution("2.5*4").NumericResult);
        }

        [Test]
        public void ItShoutComuputeExpressionWithParenthesis()
        {
            var solution = _interpreter.FindSolution("7*(2+3)");
            Assert.AreEqual(35, solution.NumericResult);
            Assert.AreEqual("7 * (2 + 3)", solution.Steps[0].FullExpression.ToString());
            Assert.AreEqual("7 * 5", solution.Steps[1].FullExpression.ToString());
        }

        [Test]
        public void ItShoutComuputeExpressionWithMultiParenthesis()
        {
            var solution = _interpreter.FindSolution("(2-(3+4)*6)+0.5");
            Assert.AreEqual(-39.5, solution.NumericResult);
            Assert.AreEqual("(2 - (3 + 4) * 6) + 0.5", solution.Steps[0].FullExpression.ToString());
            Assert.AreEqual("(2 - 7 * 6) + 0.5", solution.Steps[1].FullExpression.ToString());
            Assert.AreEqual("(2 - 42) + 0.5", solution.Steps[2].FullExpression.ToString());
            Assert.AreEqual("-40 + 0.5", solution.Steps[3].FullExpression.ToString());
        }

        [Test]
        public void ItShouldComupteWhenSubtractionIsInsideParenthesis()
        {
            var solution = _interpreter.FindSolution("2*(-3+(-6))");
            Assert.AreEqual(-18, solution.NumericResult);
            Assert.AreEqual("2 * (-3 + (-6))", solution.Steps[0].FullExpression.ToString());
            Assert.AreEqual("2 * (-3 + (-6))", solution.Steps[1].FullExpression.ToString());
            Assert.AreEqual("2 * (-3 + -6)", solution.Steps[2].FullExpression.ToString());
            Assert.AreEqual("2 * -9", solution.Steps[3].FullExpression.ToString());
            Assert.AreEqual("-18", solution.Steps[4].FullExpression.ToString());
        }

        [Test]
        public void ItShouldComputeWhenNegatingExpressionInParenthesis()
        {
            var solution = _interpreter.FindSolution("-(1+2+3*4)");
            Assert.AreEqual(-15, solution.NumericResult);
            Assert.AreEqual("-(1 + 2 + 3 * 4)", solution.Steps[0].FullExpression.ToString());
            Assert.AreEqual("-(3 + 3 * 4)", solution.Steps[1].FullExpression.ToString());
            Assert.AreEqual("-(3 + 12)", solution.Steps[2].FullExpression.ToString());
        }

        [Test]
        public void ItShouldComputeExpressionsInOrderWhenOrderIsTheSame()
        {
            var solution = _interpreter.FindSolution("1+2-3+4-5");
            Assert.AreEqual(-1, solution.NumericResult);
        }

        [Test]
        public void ItShouldComputeLongComplexExpression()
        {
            var solution = _interpreter.FindSolution("0.5+0.5*(2*2)+1+2+3+4*6-(2*(-3)+4)");
            Assert.AreEqual(34.5, solution.NumericResult);
        }

    }
}
