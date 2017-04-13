using System.Collections.Generic;
using Android.Graphics;
using MathDrawer.Models;
using MathExecutor.Models;

namespace MathDrawer.Interfaces
{
    public interface IAnimationStepsDrawer
    {
        IEnumerable<IEnumerable<DrawableExpression>> GetAnimationSteps(
            IEnumerable<Step> steps,
            Typeface tf,
            int x,
            int y,
            int width,
            int height
            );
    }
}