using System;
using System.IO;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Camera = Android.Hardware.Camera;
using ImageSharp;
using Console = System.Console;
using Environment = Android.OS.Environment;
using File = Java.IO.File;
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

            var bitmap = Bitmap.CreateBitmap(b1, _textureView.Width / 2 - BX / 2, _textureView.Height / 2 - BY / 2, BX, BY);
            _imageView.SetImageBitmap(bitmap);
            using (var stream = new MemoryStream())
            {
                Console.WriteLine("compressing");
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 70, stream);
                var bitmapData = stream.ToArray();
                var img = new Image(bitmapData);
                Console.WriteLine("grayscale");
                var changed = img.Grayscale().Brightness(20).Contrast(70);

                var dir = new File(Environment.GetExternalStoragePublicDirectory(
                    Environment.DirectoryPictures), "MathScript");

                var path = Path.Combine(dir.AbsolutePath, "test.bmp");

                if (!dir.Exists())
                {
                    dir.Mkdir();
                }

                changed.Save(path);
                Console.WriteLine($"{img.Width}:{img.Height}");
            }
            _textureView.Alpha = 0;
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