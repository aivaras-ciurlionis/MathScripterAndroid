using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using MathDrawer;
using MathDrawer.Interfaces;
using MathExecutor.Expressions;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Interfaces;
using MathExecutor.Interpreter;
using MathExecutor.Models;

namespace MathScripter.Views
{
    public class ExpressionView : View
    {
        private IBaseDrawer _baseDrawer = App.Container.Resolve(typeof(BaseDrawer), "baseDrawer") as IBaseDrawer;
        private IInterpreter _interpreter = App.Container.Resolve(typeof(Interpreter), "interpreter") as IInterpreter;

        public ExpressionView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            canvas.DrawColor(Color.White);
            var p = new Paint
            {
                Color = Color.Black,
                StrokeWidth = 5,
                TextSize = 90
            };
            const string expressionText = "1+2*3+4+5+6*7+8";
            var expression = _interpreter.GetExpression(expressionText);
            _baseDrawer.DrawExpression(expression, p, canvas);
        }
    }
}