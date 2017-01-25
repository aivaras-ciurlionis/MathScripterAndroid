using System.Collections.Generic;
using System.Diagnostics;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Expressions
{
    public class RootExpression : IExpression
    {
        private Solution SolutionTracker { get; }

        protected IExpression Operand;

        public RootExpression(IExpression operand, Solution solution)
        {
            Operand = operand;
            Operand.ParentExpression = this;
            SolutionTracker = solution;
        }

        public ExpressionType Type => ExpressionType.Root;
        public int Order => 0;
        public bool CanBeExecuted() => false;
        public void AddStep(IExpression expressionBefore, IExpression expressionAfter)
        {
           ;
            var step = new Step
            {
                ComputedExpression = expressionBefore,
                ExpressionResult = expressionAfter,
                FullExpression = Operand.Clone(),
                TextExpressionResult = ToString()
            };
            Debug.WriteLine(step.TextExpressionResult);
            SolutionTracker.AddStep(step);
        }

        public IExpression Execute()
        {
            return Operand.Execute();
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
            Operand = Operand.ReplaceVariables(values);
            return Operand;
        }

        public override string ToString()
        {
            return Operand.ToString();
        }

        public IExpression Clone()
        {
            return new RootExpression(Operand, SolutionTracker);
        }
    }
}