using System;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;

namespace MathScripter.Views
{
    public class ExpressionView : View
    {
        public ExpressionView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        protected override void OnDraw(Canvas canvas)
        {
            Console.WriteLine("xx");
            base.OnDraw(canvas);
            canvas.DrawColor(Color.White);
            var p = new Paint
            {
                Color = Color.Black,
                StrokeWidth = 5,
                TextSize = 120
            };
            const string text = "12+3•4";
            var tWidth = p.MeasureText(text);
            canvas.DrawText(text, canvas.Width / 2 - tWidth / 2, canvas.Height / 2, p);
        }
    }
}