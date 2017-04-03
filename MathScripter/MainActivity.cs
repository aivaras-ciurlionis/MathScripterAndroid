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
        private ExpressionView _expressionView;
        private string _expression = "2+2";

        private readonly INetworkDataLoader _networkDataLoader =
            App.Container.Resolve(typeof(NetworkDataLoader), "networkDataLoader") as INetworkDataLoader;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            _networkDataLoader.LoadData(Assets);
            SetContentView(Resource.Layout.Main);
            _cameraButton = FindViewById<Button>(Resource.Id.cameraButton);
            _editButton = FindViewById<Button>(Resource.Id.editButton);
            _expressionView = FindViewById<ExpressionView>(Resource.Id.expressionView);
            _cameraButton.Click += _openCamera;
            _editButton.Click += _openEdit;
            _expressionView.SetExpression(_expression);
            _expressionView.SetMode(ExpressionViewMode.Steps);
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

