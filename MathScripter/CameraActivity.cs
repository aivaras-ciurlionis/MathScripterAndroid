using System;
using System.IO;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using ImageSharp;
using MathExecutor.Interfaces;
using MathExecutor.Interpreter;
using MathRecognizer;
using Camera = Android.Hardware.Camera;
using Console = System.Console;
using MathRecognizer.Interfaces;

namespace MathScripter
{
    [Activity(Label = "CameraActivity")]
    public class CameraActivity : Activity, TextureView.ISurfaceTextureListener
    {
        private Camera _camera;
        private TextureView _textureView;
        private Button _captureButton;
        private ImageView _imageView;
        private TextView _textView;
        private IRecognizer _recognizer;

        const int BX = 500;
        const int BY = 120;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Camera);

            _textureView = FindViewById<TextureView>(Resource.Id.cameraView);
            _textView = FindViewById<TextView>(Resource.Id.textView1);
            _textureView.SurfaceTextureListener = this;

            _imageView = FindViewById<ImageView>(Resource.Id.imageView1);
            _captureButton = FindViewById<Button>(Resource.Id.captureButton);
            _captureButton.Click += OnCapture;
        }

        public void OnCapture(object sender, EventArgs args)
        {
            try
            {
                _camera.StopPreview();
                _camera.Release();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            ProcessImage();
        }

        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {
            _camera = Camera.Open();
            var pr = _camera.GetParameters();
            if (pr.SupportedFocusModes != null && pr.SupportedFocusModes.Contains(Camera.Parameters.FocusModeContinuousPicture))
            {
                pr.FocusMode = Camera.Parameters.FocusModeContinuousPicture;
            }
            if (pr.SupportedFlashModes != null)
            {
                pr.FlashMode = Camera.Parameters.FlashModeTorch;
            }
            _camera.SetParameters(pr);
            var previewSize = _camera.GetParameters().PreviewSize;
            _textureView.LayoutParameters =
                new FrameLayout.LayoutParams(previewSize.Width, previewSize.Height, GravityFlags.Center);
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
            try
            {
                _camera.StopPreview();
                _camera.Release();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
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
            var bitmap = Bitmap.CreateBitmap(b1, _textureView.Width / 2 - BX / 2, _textureView.Height / 2 - BY / 2, BX,
                BY);
            Image image;
            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 90, stream);
                var bitmapData = stream.ToArray();
                image = new Image(bitmapData);
            }
            try
            {
                var equation = _recognizer.GetEquationsInImage(image);
                _textView.Text = equation;
                var result = new Intent(this, typeof(MainActivity));
                result.PutExtra("expression", equation);
                SetResult(Result.Ok, result);
                Finish();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                _textView.Text = Resources.GetString(Resource.String.ImageParsingError);
            }
        }

    }
}