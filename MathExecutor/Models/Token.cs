namespace MathExecutor.Models
{
    public class Token
    {
        public TokenType TokenType { get; set; }
        public string Value { get; set; }
        public int Level { get; set; }
        public int Order { get; set; }
    }
}