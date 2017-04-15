using System.Collections.Generic;
using MathExecutor.Models;

namespace MathExecutor.Interfaces
{
    public interface IExpression
    {
        IList<IExpression> Operands { get; }
        IExpression Execute();
        IExpression ParentExpression { get; set; }
        ExpressionType Type { get; }
        int Arity { get; }
        int Order { get; }
        bool CanBeExecuted();
        bool IsEqualTo (IExpression other);
        string Name { get; }
        void AddStep(IExpression expressionBefore, IExpression expressionAfter);
        IExpression ReplaceVariables(Dictionary<string, double> values);
        IExpression Clone(bool changeId);
        IExpression Clone();
        string Id { get; set; }
    }
}