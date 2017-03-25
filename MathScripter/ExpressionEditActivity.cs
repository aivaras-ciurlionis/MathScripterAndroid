using System;
using Android.App;
using Android.OS;
using Android.Widget;
using MathScripter.Views;

namespace MathScripter
{
    [Activity(Label = "ExpressionEditActivity")]
    public class ExpressionEditActivity : Activity
    {
        private string _expression;
        private ExpressionView _expressionView;
        private EditText _editText;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ExpressionEdit);
            _expression = Intent.GetStringExtra("expression") ?? "";
            _expressionView = FindViewById<ExpressionView>(Resource.Id.expressionView);
            _editText = FindViewById<EditText>(Resource.Id.expressionEdit);
            _expressionView.SetExpression(_expression);
            _editText.Text = _expression;
            _editText.TextChanged += _onExpressionChanged;
        }

        private void _onExpressionChanged(object sender, EventArgs args)
        {
            _expression = _editText.Text;
            _expressionView.SetExpression(_expression);
        }

    }
}