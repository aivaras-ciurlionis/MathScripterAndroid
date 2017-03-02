using System;
using Android.App;
using Android.Widget;
using Android.OS;

namespace MathScripter
{
    [Activity(Label = "MathScripter", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private Button _cameraButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
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

