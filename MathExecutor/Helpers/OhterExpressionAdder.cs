using System.Collections.Generic;
using System.Linq;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Interfaces;

namespace MathExecutor.Helpers
{
    public class OhterExpressionAdder : IOtherExpressionAdder
    {
        private IExpression AddExpression(IExpression destination, IExpression source, bool switchSigns)
        {

            IExpression added;

            if (source.ParentExpression is SubtractExpression && 
                source.ParentExpression.Operands[1] == source)
            {
                added = new SubtractExpression(destination, source);
            }
            else
            {
                added = new SumExpression(destination, source);
            }

            if (switchSigns)
            {
                if (added is SumExpression)
                {
                    added = new SubtractExpression(destination, source);
                }
                else
                {
                    added = new SumExpression(destination, source);
                }
            }
           
            destination.ParentExpression = added;
            source.ParentExpression = added;
            destination = added;
            return destination;
        }


        public IExpression AddExpressions(
            IExpression lastExpression,
            IList<IExpression> expressions,
            bool switchSigns)
        {
            return expressions.Aggregate(
                lastExpression,
                (current, reorderableExpression) => AddExpression(current, reorderableExpression, switchSigns)
              );
        }
    }
}