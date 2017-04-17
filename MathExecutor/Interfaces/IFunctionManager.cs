using System.Collections.Generic;

namespace MathExecutor.Interfaces
{
    public interface IFunctionManager
    {
        void ClearAll();
        bool AddFunction(IExpression function);
        bool RemoveFunction(int index);
        IEnumerable<double?> GetSingleGraphPoints(int index, double start, double end, double stepInterval);
        IEnumerable<IEnumerable<double?>> GetGraphPoints(int index, double start, double end, double stepInterval);
    }
}