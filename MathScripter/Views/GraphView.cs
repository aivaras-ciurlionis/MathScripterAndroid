using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using MathDrawer.Functions;
using MathDrawer.Interfaces;
using MathExecutor.Interfaces;

namespace MathScripter.Views
{
    public class GraphView : View, GestureDetector.IOnGestureListener
    {
        private readonly IGraphDrawer _graphDrawer =
            App.Container.Resolve(typeof(GraphDrawer), "graphDrawer") as IGraphDrawer;

        private const int BufferSizeX = 2000;
        private const int BufferSizeY = 2000;

        private int _lastColor;

        private Color _netColor = Color.Rgb(133, 248, 185);

        private readonly IList<Color> _colors = new List<Color>
        {
            Color.DarkBlue,
            Color.DarkRed,
            Color.Green,
            Color.Orange
        };

        private Bitmap _buffer = Bitmap.CreateBitmap(1, 1, Bitmap.Config.Argb8888);

        private readonly GestureDetector _gestureDetector;
        private float? _currentOffsetY;
        private float? _currentOffsetX;
        private bool _redraw;

        public GraphView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            var assets = context.Assets;
            var tf = Typeface.CreateFromAsset(assets, "Content/Fonts/LinLibertine_R.ttf");
            _graphDrawer.Init(BufferSizeX, BufferSizeY, _netColor, tf);
            _gestureDetector = new GestureDetector(this);
            _redraw = true;
          
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
            if (_currentOffsetX - distanceX < 0 &&
                _currentOffsetX - distanceX > -BufferSizeX + Width)
            {
                _currentOffsetX -= distanceX;
            }

            if (_currentOffsetY - distanceY < 0 &&
                _currentOffsetY - distanceY > -BufferSizeY + Height)
            {
                _currentOffsetY -= distanceY;
            }

            Console.WriteLine(_currentOffsetX);
            Console.WriteLine(_currentOffsetY);
            Console.WriteLine(Width);
            Console.WriteLine(Height);
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

        private void PrecomputeOffset()
        {
            if (Height <= 0 || Width <= 0 || _currentOffsetX != null) return;
            _currentOffsetY = Convert.ToInt32((BufferSizeY - Height) / -2);
            _currentOffsetX = Convert.ToInt32((BufferSizeX - Width) / -2);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            PrecomputeOffset();
            canvas.DrawColor(Color.White);
            if (_redraw)
            {
                DrawGraphToBuffer();
            }
            canvas.DrawBitmap(_buffer, _currentOffsetX ?? 0, _currentOffsetY ?? 0, null);
        }

        public void DrawGraphToBuffer()
        {
            _buffer.Recycle();
            _buffer = Bitmap.CreateBitmap(BufferSizeX, BufferSizeY, Bitmap.Config.Argb8888);
            var canvas = new Canvas(_buffer);
            _graphDrawer.DrawGraph(canvas, new Paint());
            _redraw = false;
        }

        public void AddExpression(IExpression expression)
        {
            _graphDrawer.AddFunction(expression, _colors[_lastColor++]);
            _redraw = true;
            Invalidate();
        }

        public void RemoveExpression(int index)
        {
            _graphDrawer.RemoveFunction(index);
            _lastColor--;
            _redraw = true;
            Invalidate();
        }

        public void Clear()
        {
            _graphDrawer.ClearGraph();
            _lastColor = 0;
            _redraw = true;
            Invalidate();
        }

    }
}