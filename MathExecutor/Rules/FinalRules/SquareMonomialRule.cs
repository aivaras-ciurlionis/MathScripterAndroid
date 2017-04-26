using System;
using System.Collections.Generic;
using System.Linq;
using MathExecutor.Expressions;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Expressions.Equality;
using MathExecutor.Expressions.Sets;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Rules.FinalRules
{
    public class SquareMonomialRule : AbstractRecursiveRule
    {
        protected override InnerRuleResult ApplyRuleInner(IExpression expression)
        {
            var left = expression.Operands[0] as Monomial;
            var right = expression.Operands[1] as Monomial;
            var name = left.Variables.ElementAt(0).Name;
            var x = new Monomial(1, new List<IVariable> { new Variable {Exponent = 1, Name = name} });
            left.Variables = null;
            if (Math.Abs(left.Coefficient - 1) < 0.001)
            {
                var r = new SqrRootExpression(right);
                if (right.Coefficient < 0)
                {
                    return new InnerRuleResult(new MemberOfExpression(x, new EmptySetExpression()));
                }
                var fullResult = new SeparationExpression(
                    new EqualityExpression(x.Clone(true), r.Clone(true)),
                    new EqualityExpression(x.Clone(true), new NegationExpression(r.Clone(true))) 
                );
                return new InnerRuleResult(fullResult);
            }
            var division = new DivisionExpression(right.Clone(true), left.Clone(true));
            if (right.Coefficient / left.Coefficient < 0)
            {
                return new InnerRuleResult(new MemberOfExpression(x, new EmptySetExpression()));
            }
            var root = new SqrRootExpression(division.Clone(true));
            var fullResult2 = new SeparationExpression(
                     new EqualityExpression(x.Clone(true), root.Clone(true)),
                     new EqualityExpression(x.Clone(true), new NegationExpression(root.Clone(true)))
                 );
            return new InnerRuleResult(fullResult2);
        }

        protected override bool CanBeApplied(IExpression expression)
        {
            if (expression.Operands == null || expression.Operands.Count < 2)
            {
                return false;
            }
            var isEquality = expression.Type == ExpressionType.Equation &&
                            expression.Arity == 2;
            var left = expression.Operands[0] as Monomial;
            var right = expression.Operands[1] as Monomial;
            
            return isEquality &&
                   left != null &&
                   right != null &&
                   right.IsNumeral() &&
                   Math.Abs(left.Coefficient) > 0.001 &&
                   left.HasSingleVariableWithExponent(2);
        }

        public override string Description => "Simle quadratic equation equation: ax^2=b";
    }
}