using System.Collections.Generic;

namespace MathExecutor.Interfaces
{
    public interface IOtherExpressionAdder
    {
        IExpression AddExpressions(IExpression lastExpression, IList<IExpression> expressions, bool switchSigns);
    }
}