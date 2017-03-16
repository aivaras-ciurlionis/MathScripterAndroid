using System;
using System.Collections.Generic;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Util;
using Android.Views;
using MathDrawer;
using MathDrawer.Interfaces;
using MathDrawer.Models;
using MathExecutor.Expressions;
using MathExecutor.Expressions.Arithmetic;
using MathExecutor.Interfaces;
using MathExecutor.Interpreter;
using MathExecutor.Models;

namespace MathScripter.Views
{
    public class ExpressionView : View
    {
        private readonly IBaseDrawer _baseDrawer =
            App.Container.Resolve(typeof(BaseDrawer), "baseDrawer") as IBaseDrawer;
        private readonly IInterpreter _interpreter =
            App.Container.Resolve(typeof(Interpreter), "interpreter") as IInterpreter;
        private readonly IElementsDrawer _elementsDrawer =
            App.Container.Resolve(typeof(ElementsDrawer), "ElementsDrawer") as IElementsDrawer;

        private readonly AssetManager _assets;

        public ExpressionView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            _assets = context.Assets;
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            canvas.DrawColor(Color.White);
            var p = new Paint
            {
                Color = Color.Black,
                StrokeWidth = 5,
                TextSize = 60
            };

            var tf = Typeface.CreateFromAsset(_assets, "LinLibertine_R.ttf");
            p.SetTypeface(tf);

            const string expressionText = "1+2+3+4+((1/3)*6)";
            var expression = _interpreter.GetExpression(expressionText);
            var steps = _interpreter.FindSolution(expressionText);
            var i = 0;
            foreach (var step in steps.Steps)
            {
                var r = new RootExpression(step.FullExpression, new Solution());
                var drawableExpressions = _baseDrawer.DrawExpression(r, p, new EquationBounds
                {
                    X = canvas.Width / 2,
                    Y = 100 + i * 100
                });
                _elementsDrawer.DrawExpressions(drawableExpressions, p, canvas);
                i++;
            }


        }
    }
}