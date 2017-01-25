using MathExecutor.Expressions;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Models;
using NUnit.Framework;

namespace MathExecutorUnitTests.ModuleIntegrationTests
{
    [TestFixture]
    public class SimpleExpressions
    {

        [Test]
        public void ItShouldEvaluateSumNumericalExpression()
        {
            var x = new Monomial(4);
            var y = new Monomial(14);
            var sum = new SumExpression(x, y);
            var sub = new SubtractExpression(y, x);
            var root = new RootExpression(sum, new Solution());
            Assert.AreEqual(18d, root.FindSolution().NumericResult);
        }

        [Test]
        public void ItShouldEvaluateSubtractNumericalExpression()
        {
            var x = new Monomial(520);
            var y = new Monomial(452);
            var sub = new SubtractExpression(y, x);
            var root = new RootExpression(sub, new Solution());
            Assert.AreEqual(-68d, root.FindSolution().NumericResult);
        }

        [Test]
        public void ItShouldEvaluateCombinedSumAndSubtractNumericalExpression()
        {
            var x = new Monomial(4);
            var y = new Monomial(6);

            var z = new Monomial(2.7);
            var w = new Monomial(2.3);

            var s1 = new SumExpression(x, y);
            var s2 = new SubtractExpression(z, w);

            var s3 = new SumExpression(s1, s2);

            var root = new RootExpression(s3, new Solution());
            var solution = root.FindSolution();
            Assert.AreEqual(10.4d, solution.NumericResult);
            Assert.AreEqual(3, solution.Steps.Count);
        }

    }
}
