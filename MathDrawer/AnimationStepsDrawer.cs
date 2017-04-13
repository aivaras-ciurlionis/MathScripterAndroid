using System.Collections.Generic;
using System.Linq;
using Android.Graphics;
using MathDrawer.Interfaces;
using MathDrawer.Models;
using MathExecutor.Expressions;
using MathExecutor.Models;

namespace MathDrawer
{
    public class AnimationStepsDrawer : IAnimationStepsDrawer
    {
        private const float SizeModifier = 0.8f;

        private readonly IBaseDrawer _baseDrawer;

        public AnimationStepsDrawer(IBaseDrawer baseDrawer)
        {
            _baseDrawer = baseDrawer;
        }

        public IEnumerable<IEnumerable<DrawableExpression>> GetAnimationSteps(
            IEnumerable<Step> steps, 
            Typeface tf,
            int x,
            int y,
            int width,
            int height
            )
        {
            var p = new TextParameters
            {
                Size = 60,
                Typeface = tf
            };
            var bounds = new EquationBounds
            {
                Height = (int) (height*SizeModifier),
                Width = (int) (width * SizeModifier),
                X = x,
                Y = y
            };
            float minSize = 1000;
            var enumerable = steps as IList<Step> ?? steps.ToList();
            foreach (var step in enumerable)
            {
                float cSize;
                var e = new RootExpression(step.FullExpression, new Solution());
                _baseDrawer.DrawExpression(e, p, bounds, 50, out cSize);
                if (cSize < minSize)
                {
                    minSize = cSize;
                }
            }
            p.Size = minSize;
            var drawableList = new List<IList<DrawableExpression>>();
            foreach (var step in enumerable)
            {
                float cSize;
                var e = new RootExpression(step.FullExpression, new Solution());
                var drawables = _baseDrawer.DrawExpression(e, p, bounds, 50, out cSize, false);
                drawableList.Add(drawables);
            }
            return drawableList;
        }
    }
}