using Android.Graphics;
using MathDrawer.Interfaces;
using MathDrawer.Models;

namespace MathDrawer.Helpers
{
    public class TextMeasurer : ITextMeasurer
    {
        public Rect GetTextBounds(string text, TextParameters parameters)
        {
            var paint = new Paint { TextSize = parameters.Size };
            paint.SetTypeface(parameters.Typeface);
            var bounds = new Rect();
            paint.GetTextBounds(text, 0, text.Length, bounds);
            return bounds;
        }

        public Rect GetTextBounds(TextParameters parameters)
        {
            return GetTextBounds("a", parameters);
        }

        public float GetGenericTextHeight(TextParameters parameters)
        {
            var paint = new Paint { TextSize = parameters.Size };
            paint.SetTypeface(parameters.Typeface);
            var bounds = new Rect();
            paint.GetTextBounds("a", 0, 1, bounds);
            return bounds.Width();
        }
    }
}