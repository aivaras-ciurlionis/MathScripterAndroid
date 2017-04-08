using System;
using System.Collections.Generic;
using System.Linq;
using MathExecutor.Expressions;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Expressions.Equality;
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
                var fullResult = new SeparationExpression(
                    new EqualityExpression(x.Clone(), r),
                    new EqualityExpression(x.Clone(), new NegationExpression(r)) 
                );
                return new InnerRuleResult(fullResult);
            }
            var division = new DivisionExpression(right, left);
            var root = new SqrRootExpression(division);
            var fullResult2 = new SeparationExpression(
                     new EqualityExpression(x.Clone(), root),
                     new EqualityExpression(x.Clone(), new NegationExpression(root))
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
                   left.HasSingleVariableWithExponent(2);
        }

        public override string Description => "Simle quadratic equation equation: ax^2=b";
    }
}