using MathExecutor.Models;

namespace MathExecutor.Interfaces
{
    public abstract class AbstractBinaryExpression : IExpression
    {
        protected IExpression LeftOperand;
        protected IExpression RightOperand;

        protected AbstractBinaryExpression(
            IExpression leftOperand,
            IExpression rightOperand)
        {
            LeftOperand = leftOperand;
            RightOperand = rightOperand;
        }

        public IExpression Execute()
        {
            LeftOperand = LeftOperand.Execute();
            RightOperand = RightOperand.Execute();
            if (!(LeftOperand is Monomial) || !(RightOperand is Monomial) || !CanBeExecuted())
                return this;
            var expressionBefore = this;
            var result = InnerExecute();
            AddStep(expressionBefore, result);
            return result;
        }

        protected abstract IExpression InnerExecute();
        public abstract bool CanBeExecuted();
        public abstract ExpressionType Type { get; }
        public abstract int Order { get; }

        public int Arity => 2;
        public IExpression ParentExpression { get; set; }
        public void AddStep(IExpression expressionBefore, IExpression expressionAfter)
        {
            ParentExpression.AddStep(expressionBefore, expressionAfter);
        }
    }
}