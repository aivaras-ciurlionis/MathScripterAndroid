using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Rules
{
    public class SignMergeRule : AbstractRecursiveRule
    {
        protected override InnerRuleResult ApplyRuleInner(IExpression expression)
        {
            var rightOperand = expression.Operands[1];
            var monomial = rightOperand as Monomial;
            if (monomial != null)
            {
                monomial.Coefficient *= -1;
            }

            if (rightOperand is NegationExpression)
            {
                rightOperand = rightOperand.Operands[0];
                rightOperand.ParentExpression = expression;
            }

            if (expression is SumExpression)
            {
                return new InnerRuleResult(new SubtractExpression(expression.Operands[0], rightOperand));
            }
            return new InnerRuleResult(new SumExpression(expression.Operands[0], expression.Operands[1])); 
        }

        protected override bool CanBeApplied(IExpression expression)
        {
            var expressionIs = (expression is SumExpression || expression is SubtractExpression);
            if (!expressionIs)
            {
                return false;
            }
            var operand = expression.Operands[1] as Monomial;
            var operandIsMonomial = (operand != null &&
                                     operand.Coefficient < 0);
            var operandIsNegation = expression.Operands[1] is NegationExpression;
            return operandIsMonomial || operandIsNegation;
        }

        public override string Description => "Merging signs";
    }
}