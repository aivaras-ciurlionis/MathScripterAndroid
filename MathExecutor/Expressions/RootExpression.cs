using System.Collections.Generic;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Expressions
{
    public class RootExpression : IExpression
    {
        private Solution SolutionTracker { get; }

        public RootExpression(IExpression operand, Solution solution)
        {
            operand.ParentExpression = this;
            Operands = new List<IExpression> {operand};
            SolutionTracker = solution;
        }

        public ExpressionType Type => ExpressionType.Root;
        public int Order => 0;
        public bool CanBeExecuted() => false;
        public string Name => "ROOT";
        public void AddStep(IExpression expressionBefore, IExpression expressionAfter)
        {
            if (expressionBefore is ParenthesisExpression)
            {
                return;
            }

            var step = new Step
            {
                ComputedExpression = expressionBefore,
                ExpressionResult = expressionAfter,
                FullExpression = Operands[0].Clone(),
                TextExpressionResult = ToString()
            };
            SolutionTracker.AddStep(step);
        }

        public IExpression Execute()
        {
            return Operands[0].Execute();
        }

        public Solution FindSolution()
        {
            var result = Execute();
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
            return SolutionTracker;
        }

        public IExpression ParentExpression { get; set; }
        public int Arity => 1;

        public IExpression ReplaceVariables(Dictionary<string, double> values)
        {
            Operands[0] = Operands[0].ReplaceVariables(values);
            return Operands[0];
        }

        public override string ToString()
        {
            return Operands[0].ToString();
        }

        public IExpression Clone()
        {
            return new RootExpression(Operands[0], SolutionTracker);
        }

        public IList<IExpression> Operands { get; }
    }
}