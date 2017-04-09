using System;
using System.Collections.Generic;
using System.Linq;
using MathExecutor.Expressions;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Expressions.Equality;
using MathExecutor.Expressions.Sets;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Rules.QuadraticRules
{
    public class QuadraticEquationRule : AbstractRecursiveRule
    {
        protected override InnerRuleResult ApplyRuleInner(IExpression expression)
        {
            var left = expression.Operands[0];
            var right = expression.Operands[1] as Monomial;
            var a1 = left.Operands[0] as Monomial;
            var b1 = left.Operands[1] as Monomial;
            if (a1 == null || b1 == null || right == null || !right.IsNumeral())
            {
                return null;
            }
            if (!a1.HasSingleVariableWithExponent(2))
            {
                return null;
            }
            var name = a1.Variables.First().Name;
            if (!b1.HasSingleVariableWithExponent(1, name))
            {
                return null;
            }
            var helperSteps = new List<IExpression>();
            var a = a1.Coefficient;
            b1.Coefficient *= left is SumExpression ? 1 : -1;
            var b = b1.Coefficient;
            var c = right.Coefficient * -1;
            var d = b * b - 4 * a * c;

            helperSteps.Add(
                new SeparationExpression(
                    new EqualityExpression(new Monomial(1, "a"), new Monomial(a)),
                    new SeparationExpression(
                         new EqualityExpression(new Monomial(1, "b"), new Monomial(b)),
                         new EqualityExpression(new Monomial(1, "c"), new Monomial(c))
                    ))
            );

            var dStrExpression = new SubtractExpression(
                new ExponentExpression(new Monomial(1, "b"), new Monomial(2)),
                new MultiplyExpression(new Monomial(4), new MultiplyExpression(new Monomial(1, "a"), new Monomial(1, "c")))
            );
            helperSteps.Add(
                 new EqualityExpression(new Monomial(1, "D"), dStrExpression)
            );

            var dExpression = new SubtractExpression(
                new ExponentExpression(new Monomial(b), new Monomial(2)),
                new MultiplyExpression(new Monomial(4), new MultiplyExpression(new Monomial(a), new Monomial(c)))
            );

            helperSteps.Add(
                new EqualityExpression(new Monomial(1, "D"), dExpression)
            );

            helperSteps.Add(
                new EqualityExpression(new Monomial(1, "D"), new Monomial(d))
            );

            var x = new Monomial(1, new List<IVariable> { new Variable { Name = name, Exponent = 1 } });
            if (d < 0)
            {
                helperSteps.Add(
                    new LessExpression(new Monomial(1, "D"), new Monomial(0))
                );
                return new InnerRuleResult(new MemberOfExpression(x, new EmptySetExpression()), false, helperSteps);
            }

            IExpression result;
            var bottom = new MultiplyExpression(new Monomial(2), new Monomial(a));

            if (Math.Abs(d) < 0.001)
            {
                result = new EqualityExpression(
                    x,
                    new DivisionExpression(
                        new Monomial(-b),
                        bottom
                        )
                    );
                return new InnerRuleResult(result, false, helperSteps);
            }

            var ds = new Monomial(d);

            helperSteps.Add(
                new MoreExpression(new Monomial(1, "D"), new Monomial(0))
            );

            var x1 = new EqualityExpression(x,
                new DivisionExpression(
                    new SubtractExpression(new Monomial(-b), new SqrRootExpression(ds)),
                    bottom
                    )
            );
            var x2 = new EqualityExpression(x,
                new DivisionExpression(
                    new SumExpression(new Monomial(-b), new SqrRootExpression(ds)),
                    bottom
                    )
            );
            result = new SeparationExpression(x1, x2);
            return new InnerRuleResult(result, false, helperSteps);
        }

        protected override bool CanBeApplied(IExpression expression)
        {
            if (!(expression is EqualityExpression))
            {
                return false;
            }
            var left = expression.Operands[0];
            var right = expression.Operands[1] as Monomial;
            return
                right != null &&
                (left is SumExpression ||
                 left is SubtractExpression);
        }

        public override string Description => "Quadratic equation";
    }
}