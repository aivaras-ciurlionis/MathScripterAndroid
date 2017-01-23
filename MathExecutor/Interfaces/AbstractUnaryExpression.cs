using MathExecutor.Models;

namespace MathExecutor.Interfaces
{
    public abstract class AbstractUnaryExpression : IExpression
    {
        protected IExpression Operand;

        protected AbstractUnaryExpression(IExpression operand)
        {
            Operand = operand;
        }

        public IExpression Execute()
        {
            Operand = Operand.Execute();
            if (!(Operand is Monomial) || !CanBeExecuted())
                return this;
            var expressionBefore = this;
            var result = InnerExecute();
            AddStep(expressionBefore, result);
            return result;
        }

        protected abstract IExpression InnerExecute();
        public abstract ExpressionType Type { get; }
        public abstract int Order { get; }
        public int Arity => 1;
        public abstract bool CanBeExecuted();
        public IExpression ParentExpression { get; set; }
        public virtual void AddStep(IExpression expressionBefore, IExpression expressionAfter)
        {
            ParentExpression.AddStep(expressionBefore, expressionAfter);
        }
    }
}