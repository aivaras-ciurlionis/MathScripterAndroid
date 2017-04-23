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
            var stepsReducer = new StepsReucer();
            _expressionFlatener = new ExpressionFlatener();
            _multiRuleChecker = new MultiRuleChecher(_expressionFlatener, elementsChanger, parentChecher);
            _finalResultChecker = new FinalResultChecker(_expressionFlatener);
            _sequentialRuleMatcher = new SequentialRuleMatcher(interpreter, _expressionFlatener, adder, stepsReducer);
            _recursiveRuleMathcer = new RecursiveRuleMatcher(_sequentialRuleMatcher, _multiRuleChecker,
                _finalResultChecker, stepsReducer);
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

        [Test]
        public void ItShouldSolveNotFull()
        {
            var expression = _parser.Parse("x^2=9");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("x = 3 , x = -3", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ItShouldSumFractionsWithEqualDenominator()
        {
            var expression = _parser.Parse("(x+6)/(x+1)+(x+4)/(x+1)");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("2x + 10 / (x + 1)", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ItShouldMulyplyComplexExpression()
        {
            var expression = _parser.Parse("(x+2)/(x+3)*((x+4)/(x+3))");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("(x^2 + 6x + 8) / (x^2 + 6x + 9)", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ItShouldMultiplyComplexExpression()
        {
            var expression = _parser.Parse("(x^2+6x+8)/(x^2+6x+8)");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("1", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ItShouldComputeDoubleParenthesisMultiplication()
        {
            var expression = _parser.Parse("x((x+1))");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("x^2 + x", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ItShouldComputeDoubleParenthesisMultiplicationWithBiquadraticPower()
        {
            var expression = _parser.Parse("x(x-2)^2+6x");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("x^3 - 4x^2 + 10x", steps.Last().FullExpression.ToString());
        }

        // Simplifying does not work well

        [Test]
        public void ItSimplifyExpression()
        {
            var expression = _parser.Parse("((x^2+2))/((x^2+2))");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("1", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ItSimplifyExpressionWithoutExtraParenthesis()
        {
            var expression = _parser.Parse("(x-26)^2+1");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("x^2 - 52x + 677", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ItSimplifyMoreComplexFraction()
        {
            var expression = _parser.Parse("((x^2+2)*(x+12))/((x^2+2))");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("x + 12", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ItSimplifyFractionWithMulyiplyElements()
        {
            var expression = _parser.Parse("(2*(x+17))/(2*(x+18))");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("(x + 17) / (x + 18)", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ItShouldKeepSameExpressionIfItCantBeSimplified()
        {
            var expression = _parser.Parse("6x^2+7x");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("6x^2 + 7x", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ItShouldLeaveSignsWhileRemovingZero()
        {
            var expression = _parser.Parse("0x^3-6x^2");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("-6x^2", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ItShouldMultiplyParenthesisAndPerformOtherActions()
        {
            var expression = _parser.Parse("x(x-3)^2-x^3");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("-6x^2 + 9x", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ItShouldReturnThatQuadraticExpressionHasNoRealSolutions()
        {
            var expression = _parser.Parse("x^2 - x + 3 = 0");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("x \u2208 \u2205", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ItShouldComputeQuadraticExpressionAndRemoveXSquared()
        {
            var expression = _parser.Parse("(x+2)^2-x^2");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("4x + 4", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ItShouldSimplifyFractionTopAndBottom()
        {
            var expression = _parser.Parse("x(x+2)/(x+(x+2)-2)");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("x^2 + 2x / 2x", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ItShouldReturnEmptySetExpression()
        {
            var expression = _parser.Parse("x = x + 6");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("x \u2208 \u2205", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ItShouldReturnRealSetExpression()
        {
            var expression = _parser.Parse("x = x + 6 - 2 * 3");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("x \u2208 R", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ItShouldReturnRealSetExpressionForEqualVariables()
        {
            var expression = _parser.Parse("a = a");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("a \u2208 R", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ItShouldReturnSameIdsForSameVariables()
        {
            var expression = _parser.Parse("(x^2+6x+9)/(2x^2+5x-3)");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
             Assert.AreEqual("(x + 3) / (2x - 1)", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ItShouldReturnSameIdsForSameVariablesReorderExpression()
        {
            var expression = _parser.Parse("24+x+x^3=36");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("x^3 + x = 12", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ItShouldReturnSameIdsForFractionsWithSameDenomitator()
        { 
            var expression = _parser.Parse("1/x+2/x+3/x+4/x");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("10 / x", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ItShouldComputeBinarySquareAndSolveLinearEquation()
        {
            var expression = _parser.Parse("(x+2)^2=x^2");
            var steps = _recursiveRuleMathcer.SolveExpression(expression);
            Assert.AreEqual("x = -1", steps.Last().FullExpression.ToString());
        }

    }
}
