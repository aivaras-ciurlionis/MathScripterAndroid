using System;
using System.Collections.Generic;
using System.Linq;
using MathExecutor.Expressions;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Expressions.Equality;
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
            var left = expression.Operands[0].Clone(true);
            var c1 = expression.Operands[1].Clone(true) as Monomial;
            var b1 = left.Operands[1].Clone(true) as Monomial;
            var a1 = left.Operands[0].Clone(true) as Monomial;

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
            var helperSteps = new List<IExpression>
            {
                expression,
                new SeparationExpression(
                    new EqualityExpression(new Monomial(1, 'a'), new Monomial(a)),
                    new SeparationExpression(
                        new EqualityExpression(new Monomial(1, 'b'), new Monomial(b)),
                        new EqualityExpression(new Monomial(1, 'c'), new Monomial(c))
                    ))
            };
            var dExpression = new SubtractExpression(
                new ExponentExpression(new Monomial(b), new Monomial(2)),
                new MultiplyExpression(new Monomial(4), new MultiplyExpression(new Monomial(a), new Monomial(c)))
            );

            helperSteps.Add(
                new EqualityExpression(new Monomial(1, 'D'), dExpression)
            );

            helperSteps.Add(
                new EqualityExpression(new Monomial(1, 'D'), new Monomial(d))
            );

          

            IExpression result;
            var m = new Monomial(1, new List<IVariable> { new Variable { Exponent = 1, Name = name } });
            if (Math.Abs(d) < 0.001)
            {
                var x = -b / 2 * a;
                helperSteps.Add(new EqualityExpression(new Monomial(1, name[0]),new Monomial(x)));
                IExpression p1;
                IExpression p2;
                if (x < 0)
                {
                    p1 = new ParenthesisExpression(new SumExpression(m.Clone(true), new Monomial(-x)));
                    p2 = new ParenthesisExpression(new SumExpression(m.Clone(true), new Monomial(-x)));
                }
                else
                {
                    p1 = new ParenthesisExpression(new SubtractExpression(m.Clone(true), new Monomial(x)));
                    p2 = new ParenthesisExpression(new SubtractExpression(m.Clone(true), new Monomial(x)));
                }
                result = new MultiplyExpression(p1, p2);
                helperSteps.Add(new EqualityExpression(expression.Clone(true), result.Clone(true)));
            }
            else
            {
                var x1 = (-b + Math.Sqrt(d)) / (2 * a);
                var x2 = (-b - Math.Sqrt(d)) / (2 * a);
                helperSteps.Add(
                    new SeparationExpression(
                    new EqualityExpression(new Monomial(1, name[0]), new Monomial(x1)),
                    new EqualityExpression(new Monomial(1, name[0]), new Monomial(x2))
                    )
                );
                var p1 = new Monomial(a);

                IExpression p2 = x1 < 0
                    ? new ParenthesisExpression(new SumExpression(m.Clone(true), new Monomial(-x1))) 
                    : new ParenthesisExpression(new SubtractExpression(m.Clone(true), new Monomial(x1)));

                IExpression p3 = x2 < 0 
                    ? new ParenthesisExpression(new SumExpression(m.Clone(true), new Monomial(-x2))) 
                    : new ParenthesisExpression(new SubtractExpression(m.Clone(true), new Monomial(x2)));

                result = new MultiplyExpression(new MultiplyExpression(p1, p2), p3);
                helperSteps.Add(new EqualityExpression(expression.Clone(true), result.Clone(true)));
            }
            return new InnerRuleResult(result, false, helperSteps);
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
            if (!m1.HasSingleVariableWithExponent(2) || Math.Abs(m1.Coefficient) < 0.001)
            {
                return false;
            }
            var name = m1.Variables.First().Name;
            return m2.HasSingleVariableWithExponent(1, name);
        }

        public override string Description => "Quadratic equation";
    }
}