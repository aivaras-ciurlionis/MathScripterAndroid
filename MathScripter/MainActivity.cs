using System;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using MathScripter.Interfaces;
using MathScripter.Models;
using MathScripter.Providers;
using MathScripter.Views;

namespace MathScripter
{
    [Activity(Label = "MathScripter", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private Button _cameraButton;
        private Button _editButton;

        private Button _expressionButton;
        private Button _resultButton;
        private Button _stepsButton;

        private ExpressionView _expressionView;
        private ExpressionViewMode _mode;
        private string _expression = "";

        private readonly INetworkDataLoader _networkDataLoader =
            App.Container.Resolve(typeof(NetworkDataLoader), "networkDataLoader") as INetworkDataLoader;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            _networkDataLoader.LoadData(Assets);
            SetContentView(Resource.Layout.Main);

            _cameraButton = FindViewById<Button>(Resource.Id.cameraButton);
            _editButton = FindViewById<Button>(Resource.Id.editButton);
            _expressionButton = FindViewById<Button>(Resource.Id.expressionButton);
            _resultButton = FindViewById<Button>(Resource.Id.resultButton);
            _stepsButton = FindViewById<Button>(Resource.Id.stepsButton);
            _expressionView = FindViewById<ExpressionView>(Resource.Id.expressionView);

            _expression = bundle?.GetString("expression") ?? "";
            _mode = ExpressionViewMode.Solution;
            if (bundle != null)
            {
                _mode = (ExpressionViewMode)bundle.GetInt("mode");
            }

            _cameraButton.Click += _openCamera;
            _editButton.Click += _openEdit;
            _expressionButton.Click += _openAnimation;
            _stepsButton.Click += _stepsButton_Click;
            _resultButton.Click += _resultButton_Click;

            _expressionView.SetExpression(_expression);
            _expressionView.SetMode(_mode);
        }

        private void _resultButton_Click(object sender, EventArgs e)
        {
            _mode = ExpressionViewMode.Solution;
            _expressionView.SetMode(ExpressionViewMode.Solution);
        }

        private void _stepsButton_Click(object sender, EventArgs e)
        {
            _mode = ExpressionViewMode.Steps;
            _expressionView.SetMode(ExpressionViewMode.Steps);
        }

        private void _expressionButton_Click(object sender, EventArgs e)
        {
            //_mode = ExpressionViewMode.Expression;
            //_expressionView.SetMode(ExpressionViewMode.Expression);
          
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutString("expression", _expression);
            outState.PutInt("mode", (int) _mode);
            base.OnSaveInstanceState(outState);
        }

        protected override void OnPause()
        {
            base.OnPause();
            _expressionView?.Clear();
        }

        private void _openCamera(object sender, EventArgs args)
        {
            StartActivityForResult(typeof(CameraActivity), 0);
        }

        private void _openAnimation(object sender, EventArgs args)
        {
            var intent = new Intent(this, typeof(AnimationActivity));
            intent.PutExtra("expression", _expression);
            StartActivityForResult(intent, 0);
        }

        private void _openEdit(object sender, EventArgs args)
        {
            var intent = new Intent(this, typeof(ExpressionEditActivity));
            intent.PutExtra("expression", _expression);
            StartActivityForResult(intent, 0);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode != Result.Ok) return;
            _expression = data.GetStringExtra("expression");
            _expressionView.SetExpression(_expression);
        }

    }
}

