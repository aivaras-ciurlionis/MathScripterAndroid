using System;
using System.Collections.Generic;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Parser
{
    public class TokenParser : ITokenParser
    {
        private readonly ISymbolTypeChecker _symbolTypeChecker;
        private readonly ITokenCreator _tokenCreator;

        public TokenParser(ISymbolTypeChecker symbolTypeChecker, ITokenCreator tokenCreator)
        {
            _symbolTypeChecker = symbolTypeChecker;
            _tokenCreator = tokenCreator;
        }

        public IEnumerable<Token> ParseTokens(string equation)
        {
            var tokens = new List<Token>();
            var currentLevel = 0;
            var currentToken = "";
            var lastSymbolType = SymbolType.Other;
            var currentSymbolType = lastSymbolType;
            var i = 0;
            foreach (var character in equation)
            {
                currentSymbolType = _symbolTypeChecker.GetSymbolType(character);
                if (currentToken.Length > 0 && lastSymbolType != currentSymbolType && i > 0)
                {
                    tokens.Add(_tokenCreator.GetToken(lastSymbolType, currentToken, currentLevel));
                    currentToken = "";
                }

                if (currentSymbolType == SymbolType.Parenthesis)
                {
                    if (character == '(')
                    {
                        tokens.Add(_tokenCreator.GetToken(SymbolType.Parenthesis, "(", currentLevel));
                    }
                    currentLevel += character == '(' ? 1 : -1;
                }
                else
                {
                    currentToken += character;
                }
                i++;
                lastSymbolType = currentSymbolType;
            }

            if (currentToken.Length > 0)
            {
                tokens.Add(_tokenCreator.GetToken(lastSymbolType, currentToken, currentLevel));
            }

            if (currentLevel != 0)
            {
                throw new ArgumentException("Unbalanced parenthesis");
            }

            return tokens;
        }
    }
}