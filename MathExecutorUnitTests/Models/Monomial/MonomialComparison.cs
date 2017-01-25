using System.Collections.Generic;
using MathExecutor.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace MathExecutorUnitTests.Models.Monomial
{
    [TestFixture]
    public class MonomialComparison
    {
        private IVariable _v1;
        private IVariable _v2;
        private IVariable _v3;

        [SetUp]
        public void Initiate()
        {
            _v1 = Substitute.For<IVariable>();
            _v2 = Substitute.For<IVariable>();
            _v3 = Substitute.For<IVariable>();

            _v1.IsEqualTo(_v2).Returns(false);
            _v2.IsEqualTo(_v1).Returns(false);
            _v1.IsEqualTo(_v1).Returns(true);
            _v2.IsEqualTo(_v2).Returns(true);

            _v1.IsEqualTo(_v3).Returns(false);
            _v2.IsEqualTo(_v3).Returns(false);
            _v3.IsEqualTo(_v1).Returns(false);
            _v3.IsEqualTo(_v2).Returns(false);

            _v3.IsEqualTo(_v3).Returns(true);

        }

        [Test]
        public void AreVariablesEqualShouldReturnTrueIfBothAreNull()
        {
            var m1 = new MathExecutor.Models.Monomial(4);
            var m2 = new MathExecutor.Models.Monomial(5);
            Assert.True(m1.AreVariablesEqual(m2));
        }

        [Test]
        public void AreVariablesEqualShouldReturnFalseIfOneHasVariablesOtherDont()
        {
            var m1 = new MathExecutor.Models.Monomial(4, new List<IVariable> {_v1, _v2});
            var m2 = new MathExecutor.Models.Monomial(5);
            Assert.False(m1.AreVariablesEqual(m2));
        }

        [Test]
        public void AreVariablesEqualShouldReturnTrueIfVariablesMatch()
        {
            var m1 = new MathExecutor.Models.Monomial(4, new List<IVariable> { _v1});
            var m2 = new MathExecutor.Models.Monomial(5, new List<IVariable> { _v1 });
            Assert.True(m1.AreVariablesEqual(m2));
        }

        [Test]
        public void AreVariablesEqualShouldReturnFalseIfVariablesDoNotMatch()
        {
            var m1 = new MathExecutor.Models.Monomial(4, new List<IVariable> { _v2 });
            var m2 = new MathExecutor.Models.Monomial(5, new List<IVariable> { _v1 });
            Assert.False(m2.AreVariablesEqual(m1));
        }

        [Test]
        public void AreVariablesEqualShouldReturnFalseIfVariablesDoNotMatchWithDifferntCount()
        {
            var m1 = new MathExecutor.Models.Monomial(10, new List<IVariable> { _v2, _v1 });
            var m2 = new MathExecutor.Models.Monomial(10, new List<IVariable> { _v2 });
            Assert.False(m2.AreVariablesEqual(m1));
        }

        [Test]
        public void AreVariablesEqualShouldReturnFalseIfVariablesDoNotMatchWithSameCountDifferentVariables()
        {
            var m1 = new MathExecutor.Models.Monomial(10, new List<IVariable> { _v2, _v1 });
            var m2 = new MathExecutor.Models.Monomial(10, new List<IVariable> { _v2, _v3 });
            Assert.False(m2.AreVariablesEqual(m1));
            Assert.False(m1.AreVariablesEqual(m2));
        }

        [Test]
        public void AreVariablesEqualShouldReturnTrueIfOrderOfVariablesAreDifferentButTheyAreSane()
        {
            var m1 = new MathExecutor.Models.Monomial(10, new List<IVariable> { _v2, _v1, _v3 });
            var m2 = new MathExecutor.Models.Monomial(10, new List<IVariable> {_v1, _v2, _v3 });
            Assert.True(m2.AreVariablesEqual(m1));
            Assert.True(m1.AreVariablesEqual(m2));
        }

    }
}
