using System;
using System.Collections.Generic;
using System.Linq;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Rules.QuadraticRules
{
    public class PartQuadraticEquationRule : AbstractRecursiveRule
    {
        private IParentChecker _parentChecker;

        public PartQuadraticEquationRule(IParentChecker parentChecker)
        {
            _parentChecker = parentChecker;
        }

        protected override InnerRuleResult ApplyRuleInner(IExpression expression)
        {
            var a = expression.Operands[0] as Monomial;
            var b = expression.Operands[1] as Monomial;
            var name = a.Variables.First().Name;
            var isNegative = expression is SubtractExpression;
            var isNegativeParent = !_parentChecker.LeftParentIsPositive(a);
            var x = new Monomial(a.Coefficient, new List<IVariable> {new Variable {Exponent = 1, Name = name} });
            var bc = b.Clone() as Monomial;
            bc.Variables = null;
            IExpression e;
            var negativeSign = (isNegative || isNegativeParent) &&
                               !(isNegative && isNegativeParent);
            if (negativeSign)
            {
                e = new SubtractExpression(x, bc);
            }
            else
            {
                e = new SumExpression(x, bc);
            }
            var x2 = new Monomial(isNegativeParent ? -1 : 1, new List<IVariable> { new Variable { Exponent = 1, Name = name } });
            var result = new MultiplyExpression(x2, new ParenthesisExpression(e));
            return new InnerRuleResult(result, isNegativeParent);
        }

        protected override bool CanBeApplied(IExpression expression)
        {

            if (!(expression is SumExpression || expression is SubtractExpression))
            {
                return false;
            }
            var m1 = expression.Operands[0] as Monomial;
            var m2 = expression.Operands[1] as Monomial;

            if (m1 == null || m2 == null)
            {
                return false;
            }
            if (!m1.HasSingleVariableWithExponent(2) || Math.Abs(m2.Coefficient) < 0.001)
            {
                return false;
            }
            var name = m1.Variables.First().Name;
            return m2.HasSingleVariableWithExponent(1, name);
        }

        public override string Description => "Quadratic equation";
    }
}