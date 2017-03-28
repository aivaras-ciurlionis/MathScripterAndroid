using System.Linq;
using MathExecutor.Helpers;
using MathExecutor.Interfaces;
using MathExecutorUnitTests.TestHelpers;
using NUnit.Framework;

namespace MathExecutorUnitTests.Helpers
{
    [TestFixture]
    public class ExpressionFlatenerTests
    {
        private IParser _parser;
        private IExpressionFlatener _expressionFlatener;

        [SetUp]
        public void SetUp()
        {
            _parser = ClassResolver.GetParser();
            _expressionFlatener = new ExpressionFlatener();
        }

        [Test]
        public void ItShouldFlattenSingleExpressionWithMonomials()
        {
            var expression = _parser.Parse("2+3");
            var flat = _expressionFlatener.FlattenExpression(expression, false, true).ToList();
            Assert.AreEqual("2", flat[0].Expression.ToString());
            Assert.AreEqual("3", flat[1].Expression.ToString());
            Assert.AreEqual("+", flat[2].Expression.Name);
            Assert.AreEqual(0, flat[2].Level);
        }

        [Test]
        public void ItShouldFlattenSingleExpressionWithoutMonomials()
        {
            var expression = _parser.Parse("2+3*6-x");
            var flat = _expressionFlatener.FlattenExpression(expression).ToList();
            Assert.AreEqual("•", flat[0].Expression.Name);
            Assert.AreEqual("+", flat[1].Expression.Name);
            Assert.AreEqual("-", flat[2].Expression.Name);
        }

        [Test]
        public void ItShouldFlattenSingleExpressionWithParenthesisWithoutMonomials()
        {
            var expression = _parser.Parse("2(2+6)-(4+3)+x+3");
            var flat = _expressionFlatener.FlattenExpression(expression).ToList();
            Assert.AreEqual("+", flat[0].Expression.Name);
            Assert.AreEqual(1, flat[0].Level);

            Assert.AreEqual("()", flat[1].Expression.Name);
            Assert.AreEqual(1, flat[1].Level);

            Assert.AreEqual("•", flat[2].Expression.Name);
            Assert.AreEqual(0, flat[2].Level);

            Assert.AreEqual("+", flat[3].Expression.Name);
            Assert.AreEqual(1, flat[3].Level);

            Assert.AreEqual("()", flat[4].Expression.Name);
            Assert.AreEqual(1, flat[4].Level);

            Assert.AreEqual("-", flat[5].Expression.Name);
            Assert.AreEqual(0, flat[5].Level);
        }

        [Test]
        public void ItShouldFlattenAndReturnOnlyFirstLevelOfExpression()
        {
            var expression = _parser.Parse("x+1+(6-7)+(x-4)-2/3");
            var flat = _expressionFlatener.FlattenExpression(expression, true).ToList();

            Assert.AreEqual("+", flat[0].Expression.Name);
            Assert.AreEqual("+", flat[1].Expression.Name);
            Assert.AreEqual("+", flat[2].Expression.Name);
            Assert.AreEqual("-", flat[3].Expression.Name);
            Assert.AreEqual(true, flat.All(e => e.Level < 1));
        }

    }
}

