using System;
using MathExecutor.Interfaces;
using MathExecutor.Models;
using System.Linq;

namespace MathExecutor.Parser
{
    public class SymbolTypeChecker : ISymbolTypeChecker
    {
        public SymbolType GetSymbolType(char symbol)
        {
            if ("()".Contains(symbol))
            {
                return SymbolType.Parenthesis;
            }

            if ("+-*/^@%$&!:'".Contains(symbol))
            {
                return SymbolType.Symbol;
            }

            if ("=<>'".Contains(symbol))
            {
                return SymbolType.Equality;
            }

            return "1234567890.,".Contains(symbol) ? SymbolType.Numeric : SymbolType.Other;
        }
    }
}