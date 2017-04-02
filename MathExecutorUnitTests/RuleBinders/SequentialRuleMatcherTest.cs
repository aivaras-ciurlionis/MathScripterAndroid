using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using MathExecutor.Helpers;
using MathExecutor.Interfaces;
using MathExecutor.Interpreter;
using MathExecutor.RuleBinders;
using MathExecutorUnitTests.TestHelpers;
using NUnit.Framework;

namespace MathExecutorUnitTests.RuleBinders
{
    [TestFixture]
    public class SequentialRuleMatcherTest
    {
        private ISequentialRuleMatcher _sequentialRuleMatcher;
        private IParser _parser;

        [SetUp]
        public void Init()
        {
            _parser = ClassResolver.GetParser();
            var interpreter = new Interpreter(_parser);
            _sequentialRuleMatcher = new SequentialRuleMatcher(
                interpreter, 
                new ExpressionFlatener(), 
                new OhterExpressionAdder()
                );
        }

        [Test]
        public void ShouldComputeSimpleExpression()
        {
            var expression = _parser.Parse("2+2+3");
            var steps = _sequentialRuleMatcher.GetSequentialRuleSteps(expression);
            Assert.AreEqual("7", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ShouldComputeSimpleExpressionInEquality()
        {
            var expression = _parser.Parse("2+2+10=x");
            var steps = _sequentialRuleMatcher.GetSequentialRuleSteps(expression);
            Assert.AreEqual("-x = -14", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ShouldComputeSimpleExpressionInEqualityWithMultiVariables()
        {
            var expression = _parser.Parse("2+2+y+10=x+15-x");
            var steps = _sequentialRuleMatcher.GetSequentialRuleSteps(expression);
            Assert.AreEqual("0x + y = 1", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ShouldComputeExpressionWithParenthesis()
        {
            var expression = _parser.Parse("2-x+(8+(2+20))+14-6+(x+x+x)");
            var steps = _sequentialRuleMatcher.GetSequentialRuleSteps(expression);
            Assert.AreEqual("2x + 40", steps.Last().FullExpression.ToString());
        }

        [Test]
        public void ShouldComputeExpressionWithParenthesisAndEquality()
        {
            var expression = _parser.Parse("2-x+(8+(2+20))+14-6+(x+x+x)=60");
            var steps = _sequentialRuleMatcher.GetSequentialRuleSteps(expression);
            Assert.AreEqual("2x = 20", steps.Last().FullExpression.ToString());
        }

    }
}
