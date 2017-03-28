using System;
using System.Collections.Generic;
using System.Linq;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Interfaces;
using MathExecutor.Models;
using NUnit.Framework;

namespace MathExecutorUnitTests.Expressions.Arithmetic
{
    [TestFixture]
    public class DivisionExpressionTest
    {
        [Test]
        public void ItShouldDivideCoeficients()
        {
            var top = new Monomial(6);
            var bot = new Monomial(3);
            var division = new DivisionExpression(top, bot);
            var result = division.Execute() as Monomial;
            Assert.AreEqual(2, result.Coefficient);
        }

        [Test]
        public void ItShouldDivideNegativeCoeficients()
        {
            var top = new Monomial(-666);
            var bot = new Monomial(222);
            var division = new DivisionExpression(top, bot);
            var result = division.Execute() as Monomial;
            Assert.AreEqual(-3, result.Coefficient);
        }

        [Test]
        public void ItShouldDivideRationalCoefficients()
        {
            var top = new Monomial(-2.5);
            var bot = new Monomial(-0.5);
            var division = new DivisionExpression(top, bot);
            var result = division.Execute() as Monomial;
            Assert.AreEqual(5, result.Coefficient);
        }

        [Test]
        public void ItShouldThrowExceptionOnDivisionByZero()
        {
            var top = new Monomial(60);
            var bot = new Monomial(0);
            var division = new DivisionExpression(top, bot);
            Assert.Throws<ArithmeticException>(() => division.Execute());
        }

        [Test]
        public void ItShouldDivideTwoSingleVariables()
        {
            var x = new Variable { Name = "x", Exponent = 2 };
            var x2 = new Variable { Name = "x", Exponent = 1 };
            var top = new Monomial(1, new List<IVariable> { x });
            var bot = new Monomial(1, new List<IVariable> { x2 });
            var division = new DivisionExpression(top, bot);
            var result = division.Execute() as Monomial;
            Assert.AreEqual(1, result.Coefficient);
            Assert.AreEqual("x", result.Variables.ElementAt(0).Name);
            Assert.AreEqual(1, result.Variables.ElementAt(0).Exponent);
        }

        [Test]
        public void ItShouldDivideTwoSingleVariablesAndRemoveThemIfExponentsAreEqual()
        {
            var x = new Variable { Name = "a", Exponent = 6 };
            var x2 = new Variable { Name = "a", Exponent = 6 };
            var top = new Monomial(2, new List<IVariable> { x });
            var bot = new Monomial(1, new List<IVariable> { x2 });
            var division = new DivisionExpression(top, bot);
            var result = division.Execute() as Monomial;
            Assert.AreEqual(2, result.Coefficient);
            Assert.AreEqual(0, result.Variables.Count());
            Assert.AreEqual("2", result.ToString());
        }

        [Test]
        public void ItShouldLeaveSameExpressionIfVariablesDiffer()
        {
            var x = new Variable { Name = "x", Exponent = 2 };
            var x2 = new Variable { Name = "y", Exponent = 3 };
            var top = new Monomial(1, new List<IVariable> { x });
            var bot = new Monomial(1, new List<IVariable> { x2 });
            var division = new DivisionExpression(top, bot);
            var result = division.Execute();
            Assert.AreEqual("x^2 / y^3", result.ToString());
        }

        [Test]
        public void ItShouldDivideMultipleVariables()
        {
            var xtop = new Variable { Name = "x", Exponent = 2 };
            var ytop = new Variable { Name = "y", Exponent = 3 };
            var xbot = new Variable { Name = "x", Exponent = 1 };
            var ybot = new Variable { Name = "y", Exponent = 1 };

            var top = new Monomial(1, new List<IVariable> { xtop, ytop });
            var bot = new Monomial(1, new List<IVariable> { xbot, ybot });
            var division = new DivisionExpression(top, bot);
            var result = division.Execute() as Monomial;
            Assert.AreEqual(2, result.Variables.Count());
            Assert.AreEqual("xy^2", result.ToString());
        }

        [Test]
        public void ItShouldMoveVariableWithNegativeCoefficientToBottom()
        {
            var xtop = new Variable { Name = "x", Exponent = 2 };
            var xbot = new Variable { Name = "x", Exponent = 4 };

            var top = new Monomial(1, new List<IVariable> { xtop });
            var bot = new Monomial(1, new List<IVariable> { xbot });
            var division = new DivisionExpression(top, bot);
            var result = division.Execute();
            Assert.AreEqual("1 / x^2", result.ToString());
        }

        [Test]
        public void ItShouldDivideVariousCoefficients()
        {
            var xtop = new Variable { Name = "x", Exponent = 2 };
            var xbot = new Variable { Name = "x", Exponent = 5 };

            var ybot = new Variable { Name = "y", Exponent = 4 };

            var ztop = new Variable { Name = "z", Exponent = 2 };
            var zbot = new Variable { Name = "z", Exponent = 1 };

            var wtop = new Variable { Name = "w", Exponent = 7 };
            var wbot = new Variable { Name = "w", Exponent = 7 };

            var top = new Monomial(1, new List<IVariable> { xtop, ztop, wtop });
            var bot = new Monomial(1, new List<IVariable> { xbot, ybot, zbot, wbot });

            var division = new DivisionExpression(top, bot);
            var result = division.Execute();
            Assert.AreEqual("z / y^4x^3", result.ToString());
        }

    }
}
