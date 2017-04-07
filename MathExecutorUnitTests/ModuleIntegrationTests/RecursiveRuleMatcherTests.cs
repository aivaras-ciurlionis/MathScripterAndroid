using System.Linq;
using MathExecutor.Helpers;
using MathExecutor.Interfaces;
using MathExecutor.Interpreter;
using MathExecutor.RuleBinders;
using MathExecutorUnitTests.TestHelpers;
using NUnit.Framework;

namespace MathExecutorUnitTests.ModuleIntegrationTests
{
    [TestFixture]
    public class RecursiveRuleMatcherTests
    {
        private IRecursiveRuleMathcer _recursiveRuleMathcer;
        private ISequentialRuleMatcher _sequentialRuleMatcher;
        private IMultiRuleChecker _multiRuleChecker;
        private IFinalResultChecker _finalResultChecker;
        private IExpressionFlatener _expressionFlatener;
        private IParser _parser;

        [SetUp]
        public void Init()
        {
            _parser = ClassResolver.GetParser();
            var interpreter = new Interpreter(_parser);
            var adder = new OhterExpressionAdder();
            var elementsChanger = new ElementsChanger();
            var parentChecher = new ParentChecker();
            _expressionFlatener = new ExpressionFlatener();
            _multiRuleChecker = new MultiRuleChecher(_expressionFlatener, elementsChanger, parentChecher);
            _finalResultChecker = new FinalResultChecker(_expressionFlatener);
            _sequentialRuleMatcher = new SequentialRuleMatcher(interpreter, _expressionFlatener, adder);
            _recursiveRuleMathcer = new RecursiveRuleMatcher(_sequentialRuleMatcher, _multiRuleChecker, _finalResultChecker);
        }

        [Test]
        public void ItShouldComputeNumericExpression()
        {
            var expression = _parser.Parse("2+2");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("4", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ItShouldSolveComplexNumericExpression()
        {
            var expression = _parser.Parse("2+2+2*(6+3+11)-43");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("1", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ItShouldSolveSimpleExpressionWithVariables()
        {
            var expression = _parser.Parse("x-12=26");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("x = 38", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ItShouldSimplifyExpressionToShortestPossible()
        {
            var expression = _parser.Parse("x+14+x-12");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("2x + 2", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ItShouldSolveLinearEquation()
        {
            var expression = _parser.Parse("a-12=-a-6");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("a = 3", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ItShouldKeepLinearEquationSimplified()
        {
            var expression = _parser.Parse("a-12=-a-6+b");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("2a - b = 6", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ItSimplifyWithParenthesisRemoval()
        {
            var expression = _parser.Parse("17-(-1+4+3-y)");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("y + 11", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ItShouldMultiplyNumberBeforeParenthesis()
        {
            var expression = _parser.Parse("2-3*(x-y)");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("-3x + 3y + 2", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ItShouldMultiplyParenthesis()
        {
            var expression = _parser.Parse("(a+b)*(c-d)-ac+ad");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("bc - bd", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ItShouldSolveQuadraticEquation()
        {
            var expression = _parser.Parse("x^2-x-2=0");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("x = -1 , x = 2", steps.Last().FullExpression.ToString());
        }

        //[Test]
        //public void ItShouldSolveNotFull()
        //{
        //    var expression = _parser.Parse("x^2=9");
        //    var steps = _recursiveRuleMathcer.SolveExpression(expression);
        //    Assert.AreEqual("x = -1 , x = 2", steps.Last().FullExpression.ToString());
        //}

        [Test]
        public void ItShouldMultiplyTwoFractions()
        {
            var expression = _parser.Parse("((x+3)/(x+1))*((x+4)/(x+2))");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("(x^2 + 7x + 12) / (x^2 + 3x + 2)", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ItShouldSumFractionsWithEqualDenominator()
        {
            var expression = _parser.Parse("(x+6)/(x+1)+(x+4)/(x+1)");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("2x + 10 / (x + 1)", steps.Last().FullExpression.ToString());
        }

    }
}
