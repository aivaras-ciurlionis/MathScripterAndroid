using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using ImageSharp;
using MathRecognizer;
using Camera = Android.Hardware.Camera;
using Console = System.Console;
using MathRecognizer.Interfaces;
using MathScripter.Providers;
using Color = Android.Graphics.Color;

namespace MathScripter
{
    [Activity(Label = "CameraActivity")]
    public class CameraActivity : Activity, TextureView.ISurfaceTextureListener, GestureDetector.IOnGestureListener,
        SeekBar.IOnSeekBarChangeListener
    {
        private Camera _camera;
        private TextureView _textureView;
        private Button _captureButton;
        private Button _editBitmapButton;
        private ImageView _imageView;
        private TextView _textView;
        private TextView _contrastText;
        private SeekBar _contrastSlider;
        private LinearLayout _editPanel;

        private TextView _brightnessText;
        private SeekBar _brightnessSlider;

        private IRecognizer _recognizer;

        private SurfaceTexture _surfaceTexture;

        private GestureDetector _gestureDetector;

        private bool _editing;
        private Bitmap _editableBitmap;
        private Bitmap _editedBitmap;

        private int _boxX;
        private int _boxY;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Camera);
            var prefs = GetPreferences(FileCreationMode.Private);
            _boxX = prefs.GetInt("boxX", 500);
            _boxY = prefs.GetInt("boxY", 200);
            _textureView = FindViewById<TextureView>(Resource.Id.cameraView);
            _textView = FindViewById<TextView>(Resource.Id.textView1);
            _textView.Text = "Point to equation an tap capture.";
            _textureView.SurfaceTextureListener = this;
            _gestureDetector = new GestureDetector(this);

            _imageView = FindViewById<ImageView>(Resource.Id.imageView1);
            _captureButton = FindViewById<Button>(Resource.Id.captureButton);
            _editBitmapButton = FindViewById<Button>(Resource.Id.editBitmapButton);
            _contrastText = FindViewById<TextView>(Resource.Id.contrastText);
            _contrastSlider = FindViewById<SeekBar>(Resource.Id.contrastBar);
            _brightnessText = FindViewById<TextView>(Resource.Id.brightnessText);
            _brightnessSlider = FindViewById<SeekBar>(Resource.Id.brightnessBar);
            _editPanel = FindViewById<LinearLayout>(Resource.Id.bottomEditPanel);

            _contrastSlider.Max = 100;
            _brightnessSlider.Max = 255 * 2;
            _brightnessSlider.Progress = 255;
            _editPanel.Visibility = ViewStates.Invisible;
            _editBitmapButton.Click += OpenBitmapEdit;
            _captureButton.Click += OnCapture;
            _contrastSlider.SetOnSeekBarChangeListener(this);
            _brightnessSlider.SetOnSeekBarChangeListener(this);
        }

        private void ReleaseCamera()
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
        }

        public void OnCapture(object sender, EventArgs args)
        {
            if (_editing)
            {
                ProcessImage(_editedBitmap);
            }
            else
            {
                DirectCapture();
            }
        }

        public void OnScroll()
        {
            _imageView.LayoutParameters = new FrameLayout.LayoutParams(_boxX, _boxY, GravityFlags.Center);
            _imageView.RequestLayout();
        }


        public void RunCamera(SurfaceTexture surface)
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

        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {
            RunCamera(surface);
            var previewSize = _camera.GetParameters().PreviewSize;
            _surfaceTexture = surface;

            _imageView.SetX(_textureView.GetX());
            _imageView.SetY(_textureView.GetY());
            _imageView.LayoutParameters = new FrameLayout.LayoutParams(_boxX, _boxY, GravityFlags.Center);
            _imageView.RequestLayout();
            _textureView.LayoutParameters =
              new FrameLayout.LayoutParams(previewSize.Width, previewSize.Height, GravityFlags.Center);
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

        private Bitmap GetBoundedBitmap()
        {
            if (!_textureView.IsAvailable) return null;
            var b1 = _textureView.GetBitmap(_textureView.Width, _textureView.Height);

            var bitmap = Bitmap.CreateBitmap(b1, _textureView.Width / 2 - _boxX / 2, _textureView.Height / 2 - _boxY / 2, _boxX, _boxY);
            return bitmap;
        }

        private void CancelBitmapEdit()
        {
            RunCamera(_surfaceTexture);
            _textureView.Visibility = ViewStates.Visible;
            _editing = false;
            _editBitmapButton.Text = "Edit";
            _editPanel.Visibility = ViewStates.Invisible;
            _textView.SetTextColor(Color.White);
            _imageView.SetImageResource(Resource.Drawable.rect);
            _textView.Text = "Point to equation an tap capture.";
        }

        private void StartBitmapEdit()
        {
            _editableBitmap = GetBoundedBitmap();
            _editedBitmap = _editableBitmap.Copy(_editableBitmap.GetConfig(), true);
            ReleaseCamera();
            _textureView.Visibility = ViewStates.Gone;
            _imageView.SetImageBitmap(_editableBitmap);
            _editing = true;
            _textView.Text = "Adjust image using sliders below";
            _textView.SetTextColor(Color.Black);
            _editBitmapButton.Text = "Cancel";
            _editPanel.Visibility = ViewStates.Visible;
        }

        private void OpenBitmapEdit(object sender, EventArgs args)
        {
            if (_editing)
            {
                CancelBitmapEdit();
            }
            else
            {
                StartBitmapEdit();
            }
        }

        public void DirectCapture()
        {
            ReleaseCamera();
            var bitmap = GetBoundedBitmap();
            ProcessImage(bitmap);
        }

        public void ProcessImage(Bitmap bitmap)
        {
            _recognizer = App.Container.Resolve(typeof(Recognizer), "recognizer") as IRecognizer;
            if (bitmap == null)
            {
                return;
            }
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
                if (!_editing)
                {
                    RunCamera(_surfaceTexture);
                }
            }
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            _gestureDetector.OnTouchEvent(e);
            return true;
        }


        public bool OnDown(MotionEvent e)
        {
            return false;
        }

        public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            return false;
        }

        public void OnLongPress(MotionEvent e)
        {
        }

        public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
        {
            if (_editing)
            {
                return false;
            }
            var newX = _boxX - distanceX;
            var newY = _boxY + distanceY;
            if (!(newX * newY < 150000) || !(newX > 50) || !(newY > 50)) return false;
            _boxX = (int)newX;
            _boxY = (int)newY;
            OnScroll();
            return true;
        }

        public void OnShowPress(MotionEvent e)
        {
        }

        public bool OnSingleTapUp(MotionEvent e)
        {
            return false;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            var prefs = GetPreferences(FileCreationMode.Private);
            var editor = prefs.Edit();
            editor.PutInt("boxX", _boxX);
            editor.PutInt("boxY", _boxY);
            editor.Commit();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutInt("boxX", _boxX);
            outState.PutInt("boxY", _boxY);
            base.OnSaveInstanceState(outState);
        }

        public void OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
        {
            _editedBitmap?.Recycle();
            _editedBitmap = BitmapProcessor
                .ChangeBitmapContrastBrightness(_editableBitmap, _contrastSlider.Progress / 20f ,_brightnessSlider.Progress - 255);
            _imageView.SetImageBitmap(_editedBitmap);
            _contrastText.Text = $"Contrast: {_contrastSlider.Progress}";
            _brightnessText.Text = $"Brightness: {_brightnessSlider.Progress - 255}";
        }

        public void OnStartTrackingTouch(SeekBar seekBar)
        {
        }

        public void OnStopTrackingTouch(SeekBar seekBar)
        {
        }
    }
}