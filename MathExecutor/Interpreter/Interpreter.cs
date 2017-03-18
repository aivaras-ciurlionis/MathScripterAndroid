using System;
using MathExecutor.Expressions;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Interpreter
{
    public class Interpreter : IInterpreter
    {
        private readonly IParser _parser;

        public Interpreter(IParser parser)
        {
            _parser = parser;
        }

        public Solution FindSolution(string expression)
        {
            var parsedExpression = _parser.Parse(expression);
            var root = new RootExpression(parsedExpression, new Solution());
            var solution = root.FindSolution();
            solution.Steps.Add(new Step { FullExpression = solution.Result });
            return solution;
        }

        public IExpression GetExpression(string expression)
        {
            var parsedExpression = _parser.Parse(expression);
            return new RootExpression(parsedExpression, new Solution());
        }
    }
}