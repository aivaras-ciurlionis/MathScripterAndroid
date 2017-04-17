using System;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using MathDrawer.Functions;
using MathDrawer.Interfaces;

namespace MathScripter.Views
{
    public class GraphView : View, GestureDetector.IOnGestureListener
    {
        private readonly IGraphDrawer _graphDrawer =
            App.Container.Resolve(typeof(GraphDrawer), "graphDrawer") as IGraphDrawer;

        private Bitmap _buffer = Bitmap.CreateBitmap(1, 1, Bitmap.Config.Argb8888);

        private readonly GestureDetector _gestureDetector;
        private float _currentOffsetY;
        private float _currentOffsetX;
        private bool _redraw;

        public GraphView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            var assets = context.Assets;
            var tf = Typeface.CreateFromAsset(assets, "Content/Fonts/LinLibertine_R.ttf");
            _graphDrawer.Init(2000, 2000, Color.Rgb(133, 248, 185), tf);
            _gestureDetector = new GestureDetector(this);
            _redraw = true;
            _currentOffsetY = 0;
            _currentOffsetX = 0;
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
            _currentOffsetY -= distanceY;
            _currentOffsetX -= distanceX;
            Invalidate();
            return true;
        }

        public void OnShowPress(MotionEvent e)
        {
        }

        public bool OnSingleTapUp(MotionEvent e)
        {
            return false;
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            canvas.DrawColor(Color.White);
            if (_redraw)
            {
                DrawGraphToBuffer();
            }
            canvas.DrawBitmap(_buffer, _currentOffsetX, _currentOffsetY, null);
        }

        public void DrawGraphToBuffer()
        {
            _buffer.Recycle();
            _buffer = Bitmap.CreateBitmap(2000, 2000, Bitmap.Config.Argb8888);
            var canvas = new Canvas(_buffer);
            _graphDrawer.DrawGraph(canvas, new Paint());
            _redraw = false;
        }

    }
}