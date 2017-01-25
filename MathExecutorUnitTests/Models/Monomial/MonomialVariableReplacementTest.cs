using System.Collections.Generic;
using MathExecutor.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace MathExecutorUnitTests.Models.Monomial
{
    [TestFixture]
    public class MonomialVariableReplacementTest
    {
        private Dictionary<string, double> _values;

        [SetUp]
        public void Initiate()
        {
            _values = new Dictionary<string, double> { { "x", 2 }, { "y", 3 } };
        }

        [Test]
        public void ShouldEvaluateMonomial()
        {
            var x = Substitute.For<IVariable>();
            x.Evaluate(2).Returns(4); // x^2
            x.Name.Returns("x");

            var y = Substitute.For<IVariable>();
            y.Evaluate(3).Returns(81); // y^4
            y.Name.Returns("y");
            var monomial = new MathExecutor.Models.Monomial (2.5, new List<IVariable> {x, y});
            Assert.True(monomial.Variables != null);
            var result = monomial.ReplaceVariables(_values) as MathExecutor.Models.Monomial;
            Assert.AreEqual(810, result.Coefficient);
        }

        [Test]
        public void ShouldReturnCoefficientIfThereAreNoVariables()
        {
            var monomial = new MathExecutor.Models.Monomial(-4.19);
            var result = monomial.ReplaceVariables(_values) as MathExecutor.Models.Monomial;
            Assert.AreEqual(-4.19, result.Coefficient);
        }

    }
}
