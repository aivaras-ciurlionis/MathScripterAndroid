using Android.Graphics;
using MathExecutor.Interfaces;

namespace MathDrawer.Interfaces
{
    public interface IGraphDrawer
    {
        float StartX { get; set; }
        float StartY { get; set; }
        int SizeX { get; set; }
        int SizeY { get; set; }
        float Scale { get; set; }
        Color NetColor { get; set; }

        void SetScale(float scale);
        void Init(int sizeX, int sizeY, Color netColor, Typeface typeface);
        void DrawGraph(Canvas c, Paint p);
        bool AddFunction(IExpression expression, Color color);
        void RemoveFunction(int index);
        void ClearGraph();
        void Reset();
    }
}