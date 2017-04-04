using MathExecutor.Helpers;
using MathExecutor.Interfaces;
using MathExecutor.Interpreter;
using MathExecutor.Rules.ParenthesisRules;
using MathExecutorUnitTests.TestHelpers;
using NUnit.Framework;

namespace MathExecutorUnitTests.Rules
{
    [TestFixture]
    public class ParenthesisMonomialMultRuleTests
    {
        private IRule _parenthesisRule;
        private IParser _parser;
        private IInterpreter _interpreter;

        [SetUp]
        public void Init()
        {
            _parenthesisRule = new ParenthesisMonomialMultRule(new ExpressionFlatener(), new ElementsChanger());
            _parser = ClassResolver.GetParser();
            _interpreter = new Interpreter(_parser);
        }

        [Test]
        public void ItShouldMultiplyAndRemoveParenthesisForSimpleExpression()
        {
            var expression = _parser.Parse("2*(2-6+3)");
            var result = _parenthesisRule.ApplyRule(expression);
            Assert.AreEqual("2 * 2 - 2 * 6 + 2 * 3", result.Expression.ToString());
        }

        [Test]
        public void ItShouldMultiplyAndRemoveParenthesisForExpressionWithOtherInside()
        {
            var expression = _parser.Parse("2*(-2-6+3*sin(x)-cos(x)+(2-6))");
            var result = _parenthesisRule.ApplyRule(expression);
            Assert.AreEqual("2 * -2 - 2 * 6 + 2 * 3 * sin (x) - 2 * cos (x) + 2 * (2 - 6)", result.Expression.ToString());
        }

    }
}
