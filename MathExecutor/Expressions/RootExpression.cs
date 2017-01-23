using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Expressions
{
    public class RootExpression : IExpression
    {
        public Solution SolutionTracker { get; set; }
        protected IExpression Operand;

        public RootExpression(IExpression operand)
        {
            Operand = operand;
        }

        public ExpressionType Type => ExpressionType.Root;
        public int Order => 0;
        public bool CanBeExecuted() => false;
        public void AddStep(IExpression expressionBefore, IExpression expressionAfter)
        {
            var step = new Step
            {
                ComputedExpression = expressionBefore,
                ExpressionResult = expressionAfter,
                FullExpression = Operand
            };
            SolutionTracker.AddStep(step);
        }

        public IExpression Execute()
        {
            var result = Operand.Execute();
            SolutionTracker.Result = result;
            if (result is Monomial)
            {
                var monomial = result as Monomial;
                SolutionTracker.HasNumericResult = monomial.IsNumeral();
                SolutionTracker.NumericResult = monomial.Coefficient;
            }
            else
            {
                SolutionTracker.HasNumericResult = false;
            }
            return result;
        }

        public IExpression ParentExpression { get; set; }
        public int Arity => 1;
    }
}