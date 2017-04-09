using System;
using System.Collections.Generic;
using MathExecutor.Interfaces;

namespace MathExecutor.Models
{
    public class RuleApplyResult
    {
        public IExpression Expression { get; set; }
        public Type RuleType { get; set; }
        public IEnumerable<IExpression> HelperExpressions { get; set; }
        public string RuleDescription { get; set; }
        public bool Applied { get; set; }
    }
}