using MathExecutor.Interfaces;

namespace MathExecutor.Models
{
    public class Token
    {
        public TokenType TokenType { get; set; }
        public string Value { get; set; }
        public int Level { get; set; }
        public int Order { get; set; }
        public int Arity { get; set; }
        public Token LefToken { get; set; }
        public Token RightToken { get; set; }

        public int Index { get; set; }
        public IExpression LeftExpression { get; set; }
        public IExpression RightExpression { get; set; }

        public static Token MultiplyToken() => new Token { Value = "*", Arity = 2, Order = 2, TokenType = TokenType.Operation};
    }
}