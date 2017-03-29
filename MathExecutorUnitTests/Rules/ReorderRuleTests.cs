﻿using MathExecutor.Helpers;
using MathExecutor.Interfaces;
using MathExecutor.Interpreter;
using MathExecutor.Rules;
using MathExecutorUnitTests.TestHelpers;
using NUnit.Framework;

namespace MathExecutorUnitTests.Rules
{
    [TestFixture]
    public class ReorderRuleTests
    {
        private IRule _reorderRule;
        private IParser _parser;
        private IInterpreter _interpreter;

        [SetUp]
        public void Init()
        {
            _reorderRule = new ReorderRule(new ExpressionFlatener());
            _parser = ClassResolver.GetParser();
            _interpreter = new Interpreter(_parser);
        }

        [Test]
        public void ShouldReorderSimpleOneLevelExpression()
        {
            var expression = _parser.Parse("2+x-2");
            var reordered = _reorderRule.ApplyRule(expression);
            Assert.AreEqual("x + 2 - 2", reordered.Expression.ToString());
            Assert.AreEqual("x + 0", _interpreter.FindSolution(reordered.Expression).Result.ToString());
        }

        [Test]
        public void ShouldReorderMoreComplexOneLevelExpression()
        {
            var expression = _parser.Parse("x+x-2-x-7+8-0.4+x");
            var reordered = _reorderRule.ApplyRule(expression);
            Assert.AreEqual("x + x - x + x + -2 - 7 + 8 - 0.4", reordered.Expression.ToString());
            Assert.AreEqual(true, reordered.Applied);
            Assert.AreEqual("2x + -1.4", _interpreter.FindSolution(reordered.Expression).Result.ToString());
        }

        [Test]
        public void ShouldReorderOneLevelExpressionWithMultiVariables()
        {
            var expression = _parser.Parse("2+x+y-x-3+y");
            var reordered = _reorderRule.ApplyRule(expression);
            Assert.AreEqual("x - x + y + y + 2 - 3", reordered.Expression.ToString());
            Assert.AreEqual("0x + 2y + -1", _interpreter.FindSolution(reordered.Expression).Result.ToString());
        }

        [Test]
        public void ShouldReorderOneLevelExpressionWithMultiplicationInside()
        {
            var expression = _parser.Parse("x+2*3*4-1+x");
            var reordered = _reorderRule.ApplyRule(expression);
            Assert.AreEqual("x + x + -1 + 2 * 3 * 4", reordered.Expression.ToString());
        }

        [Test]
        public void ShouldReorderOneLevelExpressionWithDivisionInside()
        {
            var expression = _parser.Parse("x+1-8/2+25+x/2+x");
            var reordered = _reorderRule.ApplyRule(expression);
            Assert.AreEqual("x + x + 1 + 25 - 8 / 2 + x / 2", reordered.Expression.ToString());
            var e = _interpreter.FindSolution(reordered.Expression).Result;
            reordered = _reorderRule.ApplyRule(e);
            Assert.AreEqual("2.5x + 22", _interpreter.FindSolution(reordered.Expression).Result.ToString());
        }

        [Test]
        public void ShouldReorderOneLevelExpressionWithParenthesis()
        {
            var expression = _parser.Parse("x+(46-12)+x-1-(24*26+x-y)+12");
            var reordered = _reorderRule.ApplyRule(expression);
            Assert.AreEqual("x + x + -1 + 12 + (46 - 12) - (24 * 26 + x - y)", reordered.Expression.ToString());
            Assert.AreEqual("2x + 11 + 34 - (624 + x - y)", _interpreter.FindSolution(reordered.Expression).Result.ToString());
            var e = _interpreter.FindSolution(reordered.Expression).Result;
            reordered = _reorderRule.ApplyRule(e);
            Assert.AreEqual("2x + 45 - (624 + x - y)", _interpreter.FindSolution(reordered.Expression).Result.ToString());
        }

        [Test]
        public void ShouldKeepSameOrderIfAlredyOrdered()
        {
            var expression = _parser.Parse("x+y+1+1/3");
            var reordered = _reorderRule.ApplyRule(expression);
            Assert.AreEqual("x + y + 1 + 1 / 3", reordered.Expression.ToString());
            Assert.AreEqual(false, reordered.Applied);
        }

    }
}
