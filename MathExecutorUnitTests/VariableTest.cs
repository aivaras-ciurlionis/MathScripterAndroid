using MathExecutor.Models;
using NUnit.Framework;

namespace MathExecutorUnitTests
{
    [TestFixture]
    public class VariableTest
    {
        private Variable _variable;
        private Variable _variable2;

        [SetUp]
        public void Init()
        {
            _variable = new Variable
            {
                Exponent = 4,
                Name = "a"
            };
            _variable2 = new Variable
            {
                Exponent = 4,
                Name = "a"
            };
        }

        [Test]
        public void ShouldReturnTrueIfVariablesAreEqual()
        {
            
            Assert.True(_variable.IsEqualTo(_variable2));
            _variable.Name = "ab";
            _variable2.Name = "ab";
            Assert.True(_variable.IsEqualTo(_variable2));
        }

        [Test]
        public void ShouldReturnTrueIfComparingSameVariables()
        {
            Assert.True(_variable.IsEqualTo(_variable));
            _variable.Name = "x";
            Assert.True(_variable.IsEqualTo(_variable));
        }

        [Test]
        public void ShouldReturnFalseIfVariablesHaveDifferentExponents()
        {
            _variable.Exponent = 16;
            _variable2.Exponent = 12;
            Assert.False(_variable.IsEqualTo(_variable2));
            _variable.Exponent = 15.9999999M;
            _variable2.Exponent = 16;
            Assert.False(_variable.IsEqualTo(_variable2));
        }

        [Test]
        public void ShouldReturnFalseIfVariablesHaveDifferentNames()
        {
            _variable.Name =  "x";
            _variable2.Name = "y";
            Assert.False(_variable.IsEqualTo(_variable2));
        }

        [Test]
        public void ShouldReturnTrueIfVariableNamesAreEqualInDifferentCases()
        {
            _variable.Name = "x";
            _variable2.Name = "X";
            Assert.True(_variable.IsEqualTo(_variable2));
        }

    }
}
