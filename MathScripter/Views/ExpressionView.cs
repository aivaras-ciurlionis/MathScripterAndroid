using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Util;
using Android.Views;
using MathDrawer;
using MathDrawer.Interfaces;
using MathDrawer.Models;
using MathExecutor.Expressions;
using MathExecutor.Interfaces;
using MathExecutor.Interpreter;
using MathExecutor.Models;

namespace MathScripter.Views
{
    public class ExpressionView : View
    {

        private readonly IInterpreter _interpreter =
            App.Container.Resolve(typeof(Interpreter), "interpreter") as IInterpreter;

        private readonly IExpressionDrawer _expressionDrawer =
           App.Container.Resolve(typeof(ExpressionDrawer), "expressionDrawer") as IExpressionDrawer;

        private readonly IStepsDrawer _stepsDrawer =
           App.Container.Resolve(typeof(StepsDrawer), "expressionDrawer") as IStepsDrawer;

        private string _expression = "";

        private readonly AssetManager _assets;

        public ExpressionView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            _assets = context.Assets;
        }

        public void SetExpression(string expression)
        {
            _expression = expression;
            Invalidate();
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            canvas.DrawColor(Color.White);
            var p = new Paint
            {
                Color = Color.Black,
                TextSize = 60
            };
            var tf = Typeface.CreateFromAsset(_assets, "LinLibertine_R.ttf");
            p.SetTypeface(tf);
            if (string.IsNullOrWhiteSpace(_expression)
                || !_interpreter.CanBeParsed(_expression)) return;
            var e = _interpreter.GetExpression(_expression);
            _expressionDrawer.Draw(e, p, canvas);
        }
    }
}