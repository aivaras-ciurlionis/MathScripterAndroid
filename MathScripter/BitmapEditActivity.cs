using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Widget;

namespace MathScripter
{
    [Activity(Label = "Edit image")]
    public class BitmapEditActivity : Activity
    {
        private Button _captureButton;
        private TextView _contrastText;
        private ImageView _bitmapView;
        private SeekBar _contrastSlider;
        private Bitmap _bitmap; 

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.BitmapEdit);
            _captureButton = FindViewById<Button>(Resource.Id.captureButton);
            _contrastText = FindViewById<TextView>(Resource.Id.contrastText);
            _bitmapView = FindViewById<ImageView>(Resource.Id.bitmapView);
            _contrastSlider = FindViewById<SeekBar>(Resource.Id.contrastBar);
            var pixels = Intent.GetIntArrayExtra("bitmap");
            var width = Intent.GetIntExtra("bitmapW", 1);
            var height = Intent.GetIntExtra("bitmapW", 1);
            _bitmap = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
            _bitmap.SetPixels(pixels, 0, width, 0, 0, width, height);
        }




    }
}