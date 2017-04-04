using MathExecutor.Interfaces;

namespace MathExecutor.Helpers
{
    public class ElementsChanger : IElementsChanger
    {
        public void ChangeElement(IExpression element, IExpression replacement)
        {
            if (element.ParentExpression == null)
            {
                return;
            }
            for (var i = 0; i < element.ParentExpression.Operands.Count; i++)
            {
                var operand = element.ParentExpression.Operands[i];
                if (operand != element) continue;
                element.ParentExpression.Operands[i] = replacement;
                replacement.ParentExpression = element.ParentExpression;
            }
        }
    }
}