using System;
using System.Collections.Generic;
using System.Linq;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Parser
{
    public class TokenParser : ITokenParser
    {
        private readonly ISymbolTypeChecker _symbolTypeChecker;
        private readonly ITokenCreator _tokenCreator;
        private readonly ITokenFixer _tokenFixer;
        private IList<Token> _tokens;

        public TokenParser(ISymbolTypeChecker symbolTypeChecker,
            ITokenCreator tokenCreator,
            ITokenFixer tokenFixer)
        {
            _symbolTypeChecker = symbolTypeChecker;
            _tokenCreator = tokenCreator;
            _tokenFixer = tokenFixer;
        }

        private void AddToken(Token token)
        {
            var lastToken = _tokens.LastOrDefault();
            if (lastToken != null)
            {
                lastToken.RightToken = token;
            }
            if (token == null) return;
            token.LefToken = lastToken;

            var fixedToken = _tokenFixer.GetAditionalToken(lastToken, token);
            if (fixedToken != null)
            {
                fixedToken.Index = _tokens.Count;
                fixedToken.Level = lastToken?.Level ?? token.Level;
                _tokens.Add(fixedToken);
            }
            token.Index = _tokens.Count;
            _tokens.Add(token);
        }

        public IEnumerable<Token> ParseTokens(string equation)
        {
            _tokens = new List<Token>();
            var currentLevel = 0;
            var currentToken = "";
            var lastSymbolType = SymbolType.Other;
            var currentSymbolType = lastSymbolType;
            var i = 0;
            foreach (var character in equation)
            {
                if (string.IsNullOrWhiteSpace(character.ToString())) continue;

                currentSymbolType = _symbolTypeChecker.GetSymbolType(character);
                if (currentToken.Length > 0 && lastSymbolType != currentSymbolType && i > 0)
                {
                    AddToken(_tokenCreator.GetToken(lastSymbolType, currentToken,
                        currentLevel, _tokens.LastOrDefault()));
                    currentToken = "";
                }

                if (currentSymbolType == SymbolType.Parenthesis)
                {
                    currentLevel += character == '(' ? 1 : -1;
                    if (character == '(')
                    {
                        AddToken(_tokenCreator.GetToken(SymbolType.Parenthesis, "(", currentLevel,
                            _tokens.LastOrDefault()));
                    }
                    else
                    {
                        AddToken(_tokenCreator.GetToken(SymbolType.Parenthesis, ")", currentLevel,
                            _tokens.LastOrDefault()));
                    }
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
                AddToken(_tokenCreator.GetToken(lastSymbolType, currentToken, currentLevel, _tokens.LastOrDefault()));
            }

            if (currentLevel != 0)
            {
                throw new ArgumentException("Unbalanced parenthesis");
            }

            return _tokens;
        }
    }
}