using System;
using System.Collections.Generic;
using System.IO;
using Android.App;
using Android.Widget;
using Android.OS;
using MathRecognizer.Network;
using MathScripter.Interfaces;
using MathScripter.Providers;

namespace MathScripter
{
    [Activity(Label = "MathScripter", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private Button _cameraButton;
        private readonly INetworkDataLoader _networkDataLoader =
            App.Container.Resolve(typeof(NetworkDataLoader), "networkDataLoader") as INetworkDataLoader;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            _networkDataLoader.LoadData(Assets);
            SetContentView(Resource.Layout.Main);
            _cameraButton = FindViewById<Button>(Resource.Id.cameraButton);
            _cameraButton.Click += _openCamera;
        }

        private void _openCamera(object sender, EventArgs args)
        {
            StartActivity(typeof(CameraActivity));
        }
    }
}

