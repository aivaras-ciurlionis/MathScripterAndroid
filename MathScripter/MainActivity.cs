using System;
using System.Collections.Generic;
using System.IO;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using MathRecognizer.Network;
using MathScripter.Interfaces;
using MathScripter.Providers;
using MathScripter.Views;

namespace MathScripter
{
    [Activity(Label = "MathScripter", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private Button _cameraButton;
        private ExpressionView _expressionView;

        private readonly INetworkDataLoader _networkDataLoader =
            App.Container.Resolve(typeof(NetworkDataLoader), "networkDataLoader") as INetworkDataLoader;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            _networkDataLoader.LoadData(Assets);
            SetContentView(Resource.Layout.Main);
            _cameraButton = FindViewById<Button>(Resource.Id.cameraButton);
            _expressionView = FindViewById<ExpressionView>(Resource.Id.expressionView);
            _cameraButton.Click += _openCamera;
        }

        private void _openCamera(object sender, EventArgs args)
        {
            StartActivityForResult(typeof(CameraActivity), 0);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode != Result.Ok) return;
            var expression = data.GetStringExtra("expression");
            _expressionView.SetExpression(expression);
        }

    }
}

