using System.Collections.Generic;
using System.Linq;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Parser
{
    public class MinOperationFinder : IMinOperationFinder
    {
        private static bool IsTokenSmaller(Token t1, Token t2)
        {
            if (t1.Level < t2.Level)
            {
                return true;
            }

            if (t1.Level == t2.Level &&
                   t1.Order > t2.Order)
            {
                return true;
            }

            return t1.Level == t2.Level &&
               t1.Order == t2.Order && 
               t1.Index > t2.Index;
        }

        public int FindMinOperationIndex(IList<Token> tokens)
        {
            var minIndex = 0;
            var i = 0;
            var minToken = new Token { Level = 999, Order = -1 };
            foreach (var operation in tokens)
            {
                if (operation.TokenType == TokenType.Operation &&
                    IsTokenSmaller(operation, minToken))
                {
                    minIndex = i;
                    minToken = operation;
                }
                i++;
            }
            return minIndex;
        }
    }
}