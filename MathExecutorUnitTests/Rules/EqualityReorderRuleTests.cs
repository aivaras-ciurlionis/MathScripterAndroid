using MathExecutor.Helpers;
using MathExecutor.Interfaces;
using MathExecutor.Interpreter;
using MathExecutor.Rules;
using MathExecutorUnitTests.TestHelpers;
using NUnit.Framework;

namespace MathExecutorUnitTests.Rules
{
    [TestFixture]
    public class EqualityReorderRuleTests
    {
        private IRule _reorderRule;
        private IParser _parser;
        private IInterpreter _interpreter;

        [SetUp]
        public void Init()
        {
            _reorderRule = new EqualityReorderRule(new ExpressionFlatener(), new OhterExpressionAdder());
            _parser = ClassResolver.GetParser();
            _interpreter = new Interpreter(_parser);
        }

        [Test]
        public void ShouldReorderSimpleEqualityExpression()
        {
            var expression = _parser.Parse("2+x=3");
            var reordered = _reorderRule.ApplyRule(expression);
            Assert.AreEqual("x = -2 + 3", reordered.Expression.ToString());
        }

        [Test]
        public void ShouldReorderEqualityExpressionWithVariablesOnbothSides()
        {
            var expression = _parser.Parse("2+x+3-4=3-x+2");
            var reordered = _reorderRule.ApplyRule(expression);
            Assert.AreEqual("x + x = -2 - 3 + 4 + 3 + 2", reordered.Expression.ToString());
        }

        [Test]
        public void ShouldReorderEqualityExpressionNegationExpressions()
        {
            var expression = _parser.Parse("-3.5+x=-x+4.5");
            var interpreted = _interpreter.FindSolution(expression);
            var reordered = _reorderRule.ApplyRule(interpreted.Result);
            Assert.AreEqual("x + x = 3.5 + 4.5", reordered.Expression.ToString());
        }

        [Test]
        public void ShouldReorderEqualityExpressionNegatedNumerals()
        {
            var expression = _parser.Parse("-3+x=-6+x");
            var interpreted = _interpreter.FindSolution(expression);
            var reordered = _reorderRule.ApplyRule(interpreted.Result);
            Assert.AreEqual("x - x = 3 + -6", reordered.Expression.ToString());
        }

        [Test]
        public void ShouldReorderEqualityExpressionNegatedariables()
        {
            var expression = _parser.Parse("-x+3=-x-6");
            var interpreted = _interpreter.FindSolution(expression);
            var reordered = _reorderRule.ApplyRule(interpreted.Result);
            Assert.AreEqual("-x + x = -3 - 6", reordered.Expression.ToString());
        }

        [Test]
        public void ShouldReorderEqualityExpression()
        {
            var expression = _parser.Parse("x+2=3-y+6-25-x-x+y");
            var reordered = _reorderRule.ApplyRule(expression);
            Assert.AreEqual("x + x + x + y - y = -2 + 3 + 6 - 25", reordered.Expression.ToString());
        }

        [Test]
        public void ShouldReorderComplexEqualityExpression()
        {
            var expression = _parser.Parse("x+x-1-12+12-146+8-y=0");
            var reordered = _reorderRule.ApplyRule(expression);
            Assert.AreEqual("x + x + -y = 1 + 12 - 12 + 146 - 8 + 0", reordered.Expression.ToString());
        }

        [Test]
        public void ShouldNotReorderIfAlreadyReordered()
        {
            var expression = _parser.Parse("x=12");
            var reordered = _reorderRule.ApplyRule(expression);
            Assert.AreEqual("x = 12", reordered.Expression.ToString());
        }

        [Test]
        public void ShouldNotReorderWhenZeroToRight()
        {
            var expression = _parser.Parse("x=0");
            var reordered = _reorderRule.ApplyRule(expression);
            Assert.AreEqual("x = 0", reordered.Expression.ToString());
        }

        [Test]
        public void ShouldReorderWhenNeedToSwitchSides()
        {
            var expression = _parser.Parse("2=x");
            var reordered = _reorderRule.ApplyRule(expression);
            Assert.AreEqual("-x = -2", reordered.Expression.ToString());
        }

        [Test]
        public void ShouldReorderwithOtherExpressions()
        {
            var expression = _parser.Parse("x+2-(3*sin(14))=x-6+2/(8+x)+18");
            var reordered = _reorderRule.ApplyRule(expression);
            Assert.AreEqual("x + -x - (3 * sin (14)) - 2 / (8 + x) = -2 - 6 + 18", reordered.Expression.ToString());
        }

        [Test]
        public void ShouldReorderithOtherExpressionsInNegation()
        {
            var expression = _parser.Parse("-(7+15)=2*3+x-2");
            var reordered = _reorderRule.ApplyRule(expression);
            Assert.AreEqual("-x + -(7 + 15) - 2 * 3 = -2", reordered.Expression.ToString());
        }

        [Test]
        public void ShouldReorderithOtherExpressionWithNegationInRightSide()
        {
            var expression = _parser.Parse("(7+15)=-(x+69)-x+2");
            var reordered = _reorderRule.ApplyRule(expression);
            Assert.AreEqual("x + (7 + 15) - -(x + 69) = 2", reordered.Expression.ToString());
        }

        [Test]
        public void ShouldReorderithOtherExpressionWithMultiplication()
        {
            var expression = _parser.Parse("x+2*3*4-1+x=0");
            var reordered = _reorderRule.ApplyRule(expression);
            Assert.AreEqual("x + x + 2 * 3 * 4 = 1 + 0", reordered.Expression.ToString());
        }

        [Test]
        public void ShouldReorderWhenNeedToSwitchSidesWithNegation()
        {
            var expression = _parser.Parse("-5=-x");
            var interpreted = _interpreter.FindSolution(expression);
            var reordered = _reorderRule.ApplyRule(interpreted.Result);
            Assert.AreEqual("x = 5", reordered.Expression.ToString());
        }

    }
}