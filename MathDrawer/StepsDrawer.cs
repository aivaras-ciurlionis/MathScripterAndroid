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

        public void DrawSteps(IEnumerable<Step> steps, Paint p, Canvas canvas)
        {
            var parameters = new TextParameters
            {
                Size = p.TextSize,
                Typeface = p.Typeface
            };

            const int offset = 200;

            var enumerable = steps as Step[] ?? steps.ToArray();

            var height = (canvas.Height - offset) / enumerable.Length;

            var i = 0;

            foreach (var step in enumerable)
            {
                var r = new RootExpression(step.FullExpression, new Solution());
                var drawableExpressions = _baseDrawer.DrawExpression(r, parameters, new EquationBounds
                {
                    X = canvas.Width / 2,
                    Y = offset + i * height,
                    Width = (int)(canvas.Width * 0.8),
                    Height = (int)(height - 0.1 * height)
                });
                _elementsDrawer.DrawExpressions(drawableExpressions, p, canvas);
                i++;
            }
        }
    }
}