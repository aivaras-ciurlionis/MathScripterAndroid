using System;
using System.Collections.Generic;
using System.Linq;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Rules.QuadraticRules
{
    public class QuadraticIdentityRule : AbstractRecursiveRule
    {
        private readonly IParentChecker _parentChecker;

        public QuadraticIdentityRule(IParentChecker parentChecker)
        {
            _parentChecker = parentChecker;
        }

        protected override InnerRuleResult ApplyRuleInner(IExpression expression)
        {
            var left = expression.Operands[0];
            var c1 = expression.Operands[1] as Monomial;
            var b1 = left.Operands[1] as Monomial;
            var a1 = left.Operands[0] as Monomial;

            var a = a1.Coefficient;
            var b = b1.Coefficient;
            var c = c1.Coefficient;
            var name = a1.Variables.First().Name;
            if (left is SubtractExpression)
            {
                b *= -1;
            }
            if (expression is SubtractExpression)
            {
                c *= -1;
            }
            var d = b * b - 4 * a * c;
            if (d < 0)
            {
                return null;
            }
            IExpression result;
            var m = new Monomial(1, new List<IVariable> { new Variable { Exponent = 1, Name = name } });
            if (Math.Abs(d) < 0.001)
            {
                var x = -b / 2 * a;
                var p1 = new ParenthesisExpression(new SubtractExpression(m.Clone(), new Monomial(x)));
                var p2 = new ParenthesisExpression(new SubtractExpression(m.Clone(), new Monomial(x)));
                result = new MultiplyExpression(p1, p2);
            }
            else
            {
                var x1 = (-b + Math.Sqrt(d)) / 2 * a;
                var x2 = (-b - Math.Sqrt(d)) / 2 * a;
                var p1 = new Monomial(a);
                var p2 = new ParenthesisExpression(new SubtractExpression(m.Clone(), new Monomial(x1)));
                var p3 = new ParenthesisExpression(new SubtractExpression(m.Clone(), new Monomial(x2)));
                result = new MultiplyExpression(new MultiplyExpression(p1, p2), p3);
            }
            return new InnerRuleResult(result);
        }

        protected override bool CanBeApplied(IExpression expression)
        {
            if (!(expression is SumExpression || expression is SubtractExpression))
            {
                return false;
            }
            var left = expression.Operands[0];
            if (!(left is SumExpression || left is SubtractExpression))
            {
                return false;
            }
            var m1 = left.Operands[0] as Monomial;
            var m2 = left.Operands[1] as Monomial;
            var m3 = expression.Operands[1] as Monomial;

            if (m1 == null || m2 == null || m3 == null)
            {
                return false;
            }
            if (!m3.IsNumeral())
            {
                return false;
            }
            if (!m1.HasSingleVariableWithExponent(2))
            {
                return false;
            }
            var name = m1.Variables.First().Name;
            return m2.HasSingleVariableWithExponent(1, name);
        }

        public override string Description => "Quadratic equation";
    }
}