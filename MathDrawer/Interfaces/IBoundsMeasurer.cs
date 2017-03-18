using MathDrawer.Models;
using MathExecutor.Interfaces;

namespace MathDrawer.Interfaces
{
   public  interface IBoundsMeasurer
    {
        EquationBounds GetOperandBounds(IExpression operand, TextParameters parameters); 
    }
}