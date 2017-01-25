using System;
using System.Collections.Generic;
using MathExecutor.Models;

namespace MathExecutor.Interfaces
{
    public interface IExpression
    {
        IExpression Execute();
        IExpression ParentExpression { get; set; }
        ExpressionType Type { get; }
        int Arity { get; }
        int Order { get; }
        bool CanBeExecuted();
        void AddStep(IExpression expressionBefore, IExpression expressionAfter);
        IExpression ReplaceVariables(Dictionary<string, double> values);
        IExpression Clone();
    }
}