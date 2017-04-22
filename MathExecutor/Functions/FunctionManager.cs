using System;
using System.Collections.Generic;
using System.Linq;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Functions
{
    public class FunctionManager : IFunctionManager
    {
        private readonly IInterpreter _interpreter;
        private readonly IExpressionFlatener _expressionFlatener;

        private readonly IList<IExpression> _expressions = new List<IExpression>();

        public FunctionManager(IInterpreter interpreter,
            IExpressionFlatener expressionFlatener)
        {
            _interpreter = interpreter;
            _expressionFlatener = expressionFlatener;
        }

        public void ClearAll()
        {
            _expressions.Clear();
        }

        public bool AddFunction(IExpression function)
        {
            var variableName = GetVariableName(function);
            // if there are more than 1 variable, function cannot be graphed
            if (variableName == null)
            {
                return false;
            }
            _expressions.Add(function);
            return true;
        }

        public bool RemoveFunction(int index)
        {
            if (index < 0 || index >= _expressions.Count)
            {
                return false;
            }
            _expressions.RemoveAt(index);
            return true;
        }

        private string GetVariableName(IExpression expression)
        {
            var monomials = _expressionFlatener.FlattenExpression(expression, false, true)
                .Where(e => e.Expression is Monomial)
                .Select(e => e.Expression as Monomial);

            var variables = new List<string>();
            foreach (var monomial in monomials)
            {
                variables.AddRange(monomial.GetVariables());
            }
            var distinctVariables = variables.Distinct().ToList();
            if (!distinctVariables.Any())
            {
                return "";
            }
            return distinctVariables.Count() > 1 ? null : distinctVariables.First();
        }

        private double? GetFunctionPointAt(int index, double value)
        {
            var expression = _expressions[index].Clone();
            var variableName = GetVariableName(expression);
            var replacement = new Dictionary<string, double> { { variableName, value } };
            double? result = null;
            try
            {
                var replaced = expression.ReplaceVariables(replacement);
                var solved = _interpreter.FindSolution(replaced);
                if (solved.HasNumericResult)
                {
                    result = solved.NumericResult;
                }
            }
            catch (ArithmeticException)
            {
                result = null;
            }
            return result;
        }

        public IEnumerable<double?> GetSingleGraphPoints(int index, double start, double end, double stepInterval)
        {
            if (index < 0 || index >= _expressions.Count)
            {
                return null;
            }
            var points = new List<double?>();
            var x = start;
            while (x < end)
            {
                points.Add(GetFunctionPointAt(index, x));
                x += stepInterval;
            }
            return points;
        }

        public IEnumerable<IEnumerable<double?>> GetGraphPoints(double start, double end, double stepInterval)
        {
            var points = new List<IEnumerable<double?>>();
            for (var i = 0; i < _expressions.Count; i++)
            {
                points.Add(GetSingleGraphPoints(i, start, end, stepInterval));
            }
            return points;
        }

        public bool ChangeFunctionAt(int index, IExpression expression)
        {
            if (index < 0 || index >= _expressions.Count)
            {
                return false;
            }
            _expressions[index] = expression;
            return true;
        }
    }
}