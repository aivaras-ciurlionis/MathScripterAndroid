using System;
using System.Collections.Generic;
using System.Linq;
using MathExecutor.Interfaces;
using MathExecutor.Models;

namespace MathExecutor.Parser
{
   public class MonomialResolver : IMonomialResolver
    {
        public Monomial GetMonomial(Token token)
        {
            if (token == null)
            {
                throw  new ArgumentException("Empty token!");
            }

            if (token.TokenType != TokenType.Number && token.TokenType != TokenType.Variable)
            {
                throw new ArgumentException($"Invalid token type: {token.TokenType}; value: {token.Value}");
            }

            if (token.TokenType == TokenType.Number)
            {
                return new Monomial(double.Parse(token.Value));
            }

            var variables = new List<IVariable>();

            foreach (var character in token.Value)
            {
                var variable = new Variable { Exponent = 1, Name = character.ToString() };
                if (variables.Any(v => v.Name == variable.Name))
                {
                    throw new ArgumentException($"Duplicate variable names: {token.Value}");
                }
                variables.Add(variable);
            }

            return new Monomial(1, variables);
        }
    }
}