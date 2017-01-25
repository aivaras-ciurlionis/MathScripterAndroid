using NUnit.Framework;

namespace MathExecutorUnitTests.Models.Variable
{
    [TestFixture]
    public class VariableString
    {
        [Test]
        public void ShouldConcertVariableToStringIfExponentIs1()
        {
            var variable = new MathExecutor.Models.Variable {Exponent = 1, Name = "x"};
            Assert.AreEqual("x", variable.ToString());
        }

        [Test]
        public void ShouldConcertVariableToStringIfExponentIsPositiveFraction()
        {
            var variable = new MathExecutor.Models.Variable { Exponent = 15.611, Name = "x" };
            Assert.AreEqual("x^15.611", variable.ToString());
        }

        [Test]
        public void ShouldConcertVariableToStringIfExponentIsNegativeFraction()
        {
            var variable = new MathExecutor.Models.Variable { Exponent = -7, Name = "y" };
            Assert.AreEqual("y^-7", variable.ToString());
        }

    }
}
