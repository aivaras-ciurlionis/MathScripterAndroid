using Android.Graphics;
using MathDrawer.Models;

namespace MathDrawer.Interfaces
{
    public interface ITextMeasurer
    {
        Rect GetTextBounds(string text, TextParameters parameters);
        Rect GetTextBounds(TextParameters parameters);
        float GetGenericTextHeight(TextParameters parameters);
    }
}