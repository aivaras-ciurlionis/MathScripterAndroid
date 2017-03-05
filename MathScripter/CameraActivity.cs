using System;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.IO;
using MathRecognizer;
using MathRecognizer.ImageDecoding;
using MathRecognizer.ImageProcessing;
using Camera = Android.Hardware.Camera;
using Console = System.Console;
using MathRecognizer.Interfaces;
using Environment = Android.OS.Environment;
using Path = System.IO.Path;

namespace MathScripter
{
    [Activity(Label = "CameraActivity")]
    public class CameraActivity : Activity, TextureView.ISurfaceTextureListener, Camera.IAutoFocusCallback
    {
        private Camera _camera;
        private TextureView _textureView;
        private Button _captureButton;
        private ImageView _imageView;
        private IRecognizer _recognizer;

        const int BX = 500;
        const int BY = 200;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Camera);

            _textureView = FindViewById<TextureView>(Resource.Id.cameraView);
            _textureView.SurfaceTextureListener = this;

            _imageView = FindViewById<ImageView>(Resource.Id.imageView1);
            _captureButton = FindViewById<Button>(Resource.Id.captureButton);
            _captureButton.Click += OnCapture;
        }


        public void OnCapture(object sender, EventArgs args)
        {
            // _camera.AutoFocus(this);
            ProcessImage();
        }

        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {
            _camera = Camera.Open();
            var pr = _camera.GetParameters();
            pr.FocusMode = Camera.Parameters.FocusModeContinuousPicture;
            pr.FlashMode = Camera.Parameters.FlashModeTorch;
            _camera.SetParameters(pr);
            var previewSize = _camera.GetParameters().PreviewSize;
            _textureView.LayoutParameters =
                new FrameLayout.LayoutParams(previewSize.Width / 2, previewSize.Height / 2, GravityFlags.Center);
            _imageView.SetX(_textureView.GetX());
            _imageView.SetY(_textureView.GetY());
            _imageView.LayoutParameters = new FrameLayout.LayoutParams(BX, BY, GravityFlags.Center);
            _imageView.RequestLayout();
            try
            {
                _camera.SetPreviewTexture(surface);
                _camera.StartPreview();
            }
            catch (Java.IO.IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
        {
            _camera.StopPreview();
            _camera.Release();
            return true;
        }

        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
        {
            // do nothing
        }

        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
            // do nothing
        }

        public void ProcessImage()
        {
            if (!_textureView.IsAvailable) return;
            var b1 = _textureView.GetBitmap(_textureView.Width, _textureView.Height);
            _recognizer = App.Container.Resolve(typeof(Recognizer), "recognizer") as IRecognizer;
            var bitmap = Bitmap.CreateBitmap(b1, _textureView.Width / 2 - BX / 2, _textureView.Height / 2 - BY / 2, BX, BY);
            _imageView.SetImageBitmap(bitmap);

            Console.WriteLine("start segmentation");

            var inputDir = new File(
                Environment.GetExternalStoragePublicDirectory(
                Environment.DirectoryPictures), "MathScript");
            var dir = new File(
                Environment.GetExternalStoragePublicDirectory(
                Environment.DirectoryPictures), "MathScripter");

            if (!inputDir.Exists())
            {
                dir.Mkdirs();
            }
            else
            {
                var children = dir.List();
                foreach (var child in children)
                {
                    new File(dir, child).Delete();
                }
            }
         
            var bmp = BitmapFactory.DecodeFile(Path.Combine(inputDir.AbsolutePath, "test.bmp"));

            var equations = _recognizer.GetEquationsInImage(bmp);
            Console.WriteLine("Finish segmentation");
        }

        public void OnAutoFocus(bool success, Camera camera)
        {
            if (success)
            {
                ProcessImage();
            }
            else
            {
                Console.WriteLine("Failed to focus");
            }
        }
    }
}