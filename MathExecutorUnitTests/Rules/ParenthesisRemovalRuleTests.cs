using MathExecutor.Helpers;
using MathExecutor.Interfaces;
using MathExecutor.Interpreter;
using MathExecutor.Rules;
using MathExecutor.Rules.ParenthesisRules;
using MathExecutorUnitTests.TestHelpers;
using NUnit.Framework;

namespace MathExecutorUnitTests.Rules
{
    [TestFixture]
    public class ParenthesisRemovalRuleTests
    {
        private IRule _parenthesisRule;
        private IParser _parser;
        private IInterpreter _interpreter;

        [SetUp]
        public void Init()
        {
            _parenthesisRule = new ParenthesisRemovalRule(new ExpressionFlatener(), new ElementsChanger());
            _parser = ClassResolver.GetParser();
            _interpreter = new Interpreter(_parser);
        }

        [Test]
        public void ItShouldRemoveParenthesisBeforePlus()
        {
            var expression = _parser.Parse("2+(2-x+3)");
            var result = _parenthesisRule.ApplyRule(expression);
            Assert.AreEqual("2 + 2 - x + 3", result.Expression.ToString());
        }

        [Test]
        public void ItShouldRemoveParenthesisBeforePlusWithMultiplicationInside()
        {
            var expression = _parser.Parse("2+(2-x+9*9)");
            var result = _parenthesisRule.ApplyRule(expression);
            Assert.AreEqual("2 + 2 - x + 9 * 9", result.Expression.ToString());
        }

        [Test]
        public void ItShouldRemoveParenthesisBeforePlusWithOtherParenthesisInside()
        {
            var expression = _parser.Parse("2+(-(2+2)-x+(2-1))");
            var result = _parenthesisRule.ApplyRule(expression);
            Assert.AreEqual("2 + -(2 + 2) - x + (2 - 1)", result.Expression.ToString());
        }

        [Test]
        public void ItShouldRemoveParenthesisBeforeMinusAndChangeSigns()
        {
            var expression = _parser.Parse("17-(4+3)");
            var result = _parenthesisRule.ApplyRule(expression);
            Assert.AreEqual("17 + -4 + -3", result.Expression.ToString());
        }

        [Test]
        public void ItShouldRemoveParenthesisBeforeMinusWithMinusInsideAndChangeSigns()
        {
            var expression = _parser.Parse("17-(-1+4+3-y)");
            var result = _parenthesisRule.ApplyRule(expression);
       
            Assert.AreEqual("17 + --1 + -4 + -3 - -y", result.Expression.ToString());
        }

        [Test]
        public void ItShouldRemoveParenthesisBeforeMinusWithParenthesisInside()
        {
            var expression = _parser.Parse("17-(4+3+a+b-(4*3+2))");
            var result = _parenthesisRule.ApplyRule(expression);
            Assert.AreEqual("17 + -4 + -3 + -a + -b - -(4 * 3 + 2)", result.Expression.ToString());
        }

        [Test]
        public void ItShouldRemoveParenthesisBeforeNegationAndChangeSigns()
        {
            var expression = _parser.Parse("-(x+2+a)");
            var result = _parenthesisRule.ApplyRule(expression);
            Assert.AreEqual("-x + -2 + -a", result.Expression.ToString());
        }

    }
}
