using System;
using System.Collections.Generic;
using System.IO;
using Android.App;
using Android.Widget;
using Android.OS;
using MathRecognizer.Network;

namespace MathScripter
{
    [Activity(Label = "MathScripter", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private Button _cameraButton;

        private void LoadData()
        {
            var assets = this.Assets;
            var contents = new List<string>();
            using (var sr = new StreamReader(assets.Open("layer1_weights.txt")))
            {
                contents.Add(sr.ReadToEnd());
            }
            using (var sr = new StreamReader(assets.Open("layer1_biases.txt")))
            {
                contents.Add(sr.ReadToEnd());
            }
            using (var sr = new StreamReader(assets.Open("layer2_weights.txt")))
            {
                contents.Add(sr.ReadToEnd());
            }
            using (var sr = new StreamReader(assets.Open("layer2_biases.txt")))
            {
                contents.Add(sr.ReadToEnd());
            }
            NeuralNetwork.Instance.LoadNetwork(new[] { 4096, 100, 32 }, contents.ToArray());
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            if (!NeuralNetwork.Instance.DataLoaded)
            {
                LoadData();
            }
          
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

