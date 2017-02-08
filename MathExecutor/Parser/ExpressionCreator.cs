using System;
using System.Collections.Generic;
using System.Linq;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Parser
{
    public class ExpressionCreator : IExpressionCreator
    {
        private readonly IExpressionFactory _expressionFactory;
        private readonly IMonomialResolver _monomialResolver;
        private readonly IMinOperationFinder _minOperationFinder;

        public ExpressionCreator(IExpressionFactory expressionFactory,
            IMonomialResolver monomialResolver,
            IMinOperationFinder minOperationFinder)
        {
            _expressionFactory = expressionFactory;
            _monomialResolver = monomialResolver;
            _minOperationFinder = minOperationFinder;
        }

        private IExpression ToExpression(IList<Token> tokens)
        {
            if (tokens.Count < 1)
            {
                return null;
            }

            if (tokens.Count == 1)
            {
                return _monomialResolver.GetMonomial(tokens.First());
            }

            var index = _minOperationFinder.FindMinOperationIndex(tokens);
            var nextOperation = tokens[index];

            var leftTokens = tokens.Take(index);
            var rightTokens = tokens.Skip(index + 1);

            switch (nextOperation.Arity)
            {
                case 1:
                    return _expressionFactory.GetExpression(nextOperation.Value, 
                        new List<IExpression> { ToExpression(rightTokens.ToList()) });
                case 2:
                    return _expressionFactory.GetExpression(nextOperation.Value,
                        new List<IExpression> {ToExpression(leftTokens.ToList()), ToExpression(rightTokens.ToList())});
                default:
                    return _expressionFactory.GetExpression(nextOperation.Value);
            }
        }

        public IExpression CreateExpression(IEnumerable<Token> tokens)
        {
            return ToExpression(tokens.ToList());
        }
    }
}