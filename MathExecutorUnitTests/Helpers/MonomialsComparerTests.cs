using System.Collections.Generic;
using MathExecutor.Helpers;
using MathExecutor.Interfaces;
using MathExecutor.Models;
using NUnit.Framework;

namespace MathExecutorUnitTests.Helpers
{
    [TestFixture]
    public class MonomialsComparerTests
    {
        private readonly MonomialsComparer _monomialsComparer = new MonomialsComparer();

        [Test]
        public void MonomialsAreEqualIfTheyDontHaveVariables()
        {
            var m1 = new Monomial(5);
            var m2 = new Monomial(-498);
            Assert.AreEqual(0, _monomialsComparer.Compare(m1, m2));
        }

        [Test]
        public void MonomialWithVariablesIsLarger()
        {
            var m1 = new Monomial(5, new List<IVariable> { new Variable { Exponent = 1, Name = "X" } });
            var m2 = new Monomial(-498);
            Assert.AreEqual(-1, _monomialsComparer.Compare(m1, m2));
        }

        [Test]
        public void MonomialWithMoreVariablesIsLarger()
        {
            var m1 = new Monomial(5, new List<IVariable>
            {
                new Variable { Exponent = 1, Name = "x" },
                new Variable { Exponent = 1, Name = "y" },
            });
            var m2 = new Monomial(-12, new List<IVariable>
            {
                new Variable { Exponent = 1, Name = "x" }
            });
            Assert.AreEqual(1, _monomialsComparer.Compare(m2, m1));
        }

        [Test]
        public void MonomialWithLargerExponentIsBigger()
        {
            var m1 = new Monomial(5, new List<IVariable> { new Variable { Exponent = 2, Name = "x" } });
            var m2 = new Monomial(5, new List<IVariable> { new Variable { Exponent = 1, Name = "x" } });
            Assert.AreEqual(-1, _monomialsComparer.Compare(m1, m2));
        }

        [Test]
        public void MonomialWithLargerNameIsBigger()
        {
            var m1 = new Monomial(5, new List<IVariable> { new Variable { Exponent = 2, Name = "x" } });
            var m2 = new Monomial(5, new List<IVariable> { new Variable { Exponent = 2, Name = "y" } });
            Assert.AreEqual(-1, _monomialsComparer.Compare(m1, m2));
        }

        [Test]
        public void MonomialWithSecondVariableBiggerExponentIsLarger()
        {
            var m1 = new Monomial(5, new List<IVariable>
            {
                new Variable { Exponent = 2, Name = "x" },
                new Variable { Exponent = 1, Name = "y" }
            });
            var m2 = new Monomial(5, new List<IVariable>
            {
                new Variable { Exponent = 2, Name = "x" },
                new Variable { Exponent = 4, Name = "y" }
            });
            Assert.AreEqual(1, _monomialsComparer.Compare(m1, m2));
        }

    }
}
