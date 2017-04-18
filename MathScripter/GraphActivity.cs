using System.Collections.Generic;
using Android.App;
using Android.OS;
using MathExecutor.Expressions.Equality;
using MathExecutor.Interfaces;
using MathExecutor.Interpreter;
using MathScripter.Views;

namespace MathScripter
{
    [Activity(Label = "Function Graph")]
    public class GraphActivity : Activity
    {

        private readonly IInterpreter _interpreter =
           App.Container.Resolve(typeof(Interpreter), "interpreter") as IInterpreter;

        private GraphView _graphView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Graph);
            _graphView = FindViewById<GraphView>(Resource.Id.graphView);
            var e = Intent.GetStringExtra("expression") ?? "";
            var expression = _interpreter.GetExpression(e);
            if (expression == null)
            {
                Finish();
            }
            if (expression is EqualityExpression)
            {
                var left = expression.Operands[0];
                var right = expression.Operands[1];
                _graphView.AddExpression(left);
                _graphView.AddExpression(right);
            }
            else
            {
                _graphView.AddExpression(expression);
            }
        }

    }
}