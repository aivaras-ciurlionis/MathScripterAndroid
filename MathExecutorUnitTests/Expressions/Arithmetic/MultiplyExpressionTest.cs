using System.Collections.Generic;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Interfaces;
using MathExecutor.Models;
using NUnit.Framework;

namespace MathExecutorUnitTests.Expressions.Arithmetic
{
    [TestFixture]
    public class MultiplyExpressionTest
    {
        [Test]
        public void ItShouldMultiplyCoefficients()
        {
            var m1 = new Monomial(4);
            var m2 = new Monomial(7);
            var mul = new MultiplyExpression(m1, m2);
            var result = mul.Execute() as Monomial;
            Assert.AreEqual(28, result.Coefficient);
        }

        [Test]
        public void ItShouldMultiplyVariablesWhenItIsSingle()
        {
            var v1 = new Variable {Exponent = 3, Name = "x"};
            var v2 = new Variable { Exponent = 2, Name = "x" };
            var m1 = new Monomial(2, new List<IVariable> {v1});
            var m2 = new Monomial(6, new List<IVariable> {v2});
            var mul = new MultiplyExpression(m1, m2);
            var result = mul.Execute() as Monomial;
            Assert.AreEqual("12x^5", result.ToString());
        }

        [Test]
        public void ItShouldMultiplyVariablesWhenItHasCoupleVariablesWithDifferentOrder()
        {
            var v1 = new Variable { Exponent = 3, Name = "x" };
            var v2 = new Variable { Exponent = 2, Name = "x" };
            var v3 = new Variable { Exponent = 2, Name = "y" };
            var v4 = new Variable { Exponent = 4, Name = "y" };

            var m1 = new Monomial(2.5, new List<IVariable> { v1, v4 });
            var m2 = new Monomial(-4, new List<IVariable> { v3, v2 });
            var mul = new MultiplyExpression(m1, m2);
            var result = mul.Execute() as Monomial;
            Assert.AreEqual("-10x^5y^6", result.ToString());
        }

        [Test]
        public void ItShouldMultiplyVariablesWhenTheyAreDifferent()
        {
            var v1 = new Variable { Exponent = 3, Name = "x" };
            var v3 = new Variable { Exponent = 2, Name = "y" };

            var m1 = new Monomial(2.3, new List<IVariable> { v3 });
            var m2 = new Monomial(-0.5, new List<IVariable> { v1 });
            var mul = new MultiplyExpression(m1, m2);
            var result = mul.Execute() as Monomial;
            Assert.AreEqual("-1.15y^2x^3", result.ToString());
        }

        [Test]
        public void ItShouldMultiplyVariablesWhenItHasCoupleVariablesThatAreSame()
        {
            var v1 = new Variable { Exponent = 3, Name = "x" };
            var v2 = new Variable { Exponent = 2, Name = "x" };
            var v3 = new Variable { Exponent = 2, Name = "y" };
            var v4 = new Variable { Exponent = 4, Name = "y" };
            var v5 = new Variable { Exponent = 1, Name = "z" };

            var m1 = new Monomial(2, new List<IVariable> { v1, v4 });
            var m2 = new Monomial(1, new List<IVariable> { v3, v2, v5 });
            var mul = new MultiplyExpression(m1, m2);
            var result = mul.Execute() as Monomial;
            Assert.AreEqual("2x^5y^6z", result.ToString());
        }

    }
}