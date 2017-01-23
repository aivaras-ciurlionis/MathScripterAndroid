using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MathExecutor.Models;

namespace MathExecutor.Interfaces
{
    public abstract class AbstractNullaryExpression : IExpression
    {
        public ExpressionType Type => ExpressionType.Arithmetic;
        public int Arity => 0;
        public abstract int Order { get; }
        public bool CanBeExecuted() => true;
        protected abstract decimal Value { get; }

        public IExpression Execute()
        {
            return new Monomial
            {
                Coefficient = Value
            };
        }
        public IExpression ParentExpression { get; set; }
        public void AddStep(IExpression expressionBefore, IExpression expressionAfter)
        {
        }
    }
}