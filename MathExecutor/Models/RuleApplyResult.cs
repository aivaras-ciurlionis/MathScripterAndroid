using MathExecutor.Interfaces;

namespace MathExecutor.Models
{
    public class RuleApplyResult
    {
        public IExpression Expression { get; set; }
        public string RuleDescription { get; set; }
        public bool Applied { get; set; }
    }
}