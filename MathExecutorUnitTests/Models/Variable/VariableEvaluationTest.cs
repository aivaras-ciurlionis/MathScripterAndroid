using NUnit.Framework;

namespace MathExecutorUnitTests.Models.Variable
{
    [TestFixture]
    public class VariableEvaluationTest
    {
        [Test]
        public void ShoudReturnExponentOfGivenValue()
        {
            var variable = new MathExecutor.Models.Variable
            {
                Name = "x",
                Exponent = 4
            };
            var result = variable.Evaluate(3);
            Assert.AreEqual(81d, result);
            variable.Exponent = 2;
            Assert.AreEqual(16, variable.Evaluate(4));
            variable.Exponent = 2.3;
            Assert.AreEqual(168.854, variable.Evaluate(9.3), 0.001);
            variable.Exponent = 1;
            Assert.AreEqual(8.5166, variable.Evaluate(8.5166));
        }

        [Test]
        public void ShoudltReturnExponentForNegativeNumbers()
        {
            var variable = new MathExecutor.Models.Variable
            {
                Name = "t",
                Exponent = -2
            };
            Assert.AreEqual(0.0625, variable.Evaluate(4));
            variable.Exponent = -3;
            Assert.AreEqual(-0.125, variable.Evaluate(-2));
        }
    }
}
