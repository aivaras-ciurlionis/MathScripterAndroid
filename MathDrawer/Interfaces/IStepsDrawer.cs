using System.Collections.Generic;
using Android.Graphics;
using MathExecutor.Models;

namespace MathDrawer.Interfaces
{
    public interface IStepsDrawer
    {
        void DrawSteps(IEnumerable<Step> steps, Paint p, Canvas c, int totalHeight, int totalWidth);
    }
}