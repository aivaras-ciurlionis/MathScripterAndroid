using System;
using System.Collections.Generic;
using System.Linq;
using Android.Graphics;
using MathDrawer.Interfaces;
using MathDrawer.Models;
using MathExecutor.Expressions;
using MathExecutor.Models;

namespace MathDrawer
{
    public class StepsDrawer : IStepsDrawer
    {
        private readonly IBaseDrawer _baseDrawer;
        private readonly IElementsDrawer _elementsDrawer;

        public StepsDrawer(IBaseDrawer baseDrawer,
            IElementsDrawer elementsDrawer)
        {
            _baseDrawer = baseDrawer;
            _elementsDrawer = elementsDrawer;
        }

        public void DrawSteps(IEnumerable<Step> steps, Paint p, 
            Canvas canvas, int totalHeight, int totalWidth)
        {
            var parameters = new TextParameters
            {
                Size = 60,
                Typeface = p.Typeface
            };
            
            var enumerable = steps as Step[] ?? steps.ToArray();

            const int offset = 250;
            var height = 200;
            var computedHeight = (totalHeight-offset) / enumerable.Length;
            height = Math.Min(height, computedHeight);

            var i = 0;
            foreach (var step in enumerable)
            {
                p.TextSize = 60;
                var r = new RootExpression(step.FullExpression, new Solution());
                var drawableExpressions = _baseDrawer.DrawExpression(r, parameters, new EquationBounds
                {
                    X = totalWidth / 2,
                    Y = offset + i * height,
                    Width = (int)(totalWidth * 0.8),
                    Height = (int)(height - 0.2 * height),
                }, 100);
                _elementsDrawer.DrawExpressions(drawableExpressions, p, canvas, 0);
                i++;
            }
        }
    }
}