using MathExecutor.Helpers;
using MathExecutor.Interfaces;
using MathExecutor.Interpreter;
using MathExecutor.Rules;
using MathExecutorUnitTests.TestHelpers;
using NUnit.Framework;

namespace MathExecutorUnitTests.Rules
{
    [TestFixture]
    public class SignMergeRuleTests
    {
        private IRule _signMergeRule;
        private IParser _parser;
        private IInterpreter _interpreter;

        [SetUp]
        public void Init()
        {
            _signMergeRule = new SignMergeRule();
            _parser = ClassResolver.GetParser();
            _interpreter = new Interpreter(_parser);
        }

        [Test]
        public void ShouldMergeMinusAndPlusToMinus()
        {
            var expression = _parser.Parse("x+(-3)");
            var interpreted = _interpreter.FindSolution(expression).Result;
            var reordered = _signMergeRule.ApplyRule(interpreted);
            Assert.AreEqual("x - 3", reordered.Expression.ToString());
        }

        [Test]
        public void ShouldMergeMinusAndMinusToPlus()
        {
            var expression = _parser.Parse("x-(-3)");
            var interpreted = _interpreter.FindSolution(expression).Result;
            var reordered = _signMergeRule.ApplyRule(interpreted);
            Assert.AreEqual("x + 3", reordered.Expression.ToString());
        }
    }
}
