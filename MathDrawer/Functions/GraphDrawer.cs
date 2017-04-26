using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Android.Graphics;
using MathDrawer.Interfaces;
using MathDrawer.Models;
using MathExecutor.Interfaces;

namespace MathDrawer.Functions
{
    public class GraphDrawer : IGraphDrawer
    {
        private const float ScaleMin = 0.1f;
        private const float ScaleMax = 10f;

        private const float DefaultStartX = -10f;
        private const float DefaultStartY = 10f;
        private const float DefaultSize = 1f;
        private const float DefaultPixels = 100;

        private float _pixelsPerUnit;
        private float _baseX;
        private float _baseY;
        private Typeface _typeface;

        private readonly IFunctionManager _functionManager;

        private GraphNetString _scaleString;

        private readonly IEnumerable<GraphNetString> _netStrings = new List<GraphNetString>
        {
            new GraphNetString {Width = 1, Frequency = 1, MaxSize = 2f},
            new GraphNetString {Width = 2, Frequency = 4, MaxSize = 0.25f},
            new GraphNetString {Width = 4, Frequency = 20, MaxSize = 0.05f},
            new GraphNetString {Width = 6, Frequency = 100, MaxSize = 0.01f}
        };

        private readonly IList<Color> _functionColors = new List<Color>();

        public GraphDrawer(IFunctionManager functionManager)
        {
            _functionManager = functionManager;
            Reset();
        }

        public void Init(int sizeX, int sizeY, Color netColor, Typeface typeface)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            NetColor = netColor;
            _typeface = typeface;
        }

        private float FunctionValue(double? value, bool x)
        {
            if (value == null)
            {
                return 0;
            }
            return (float)(value.Value * _pixelsPerUnit + (x ? _baseX : _baseY));
        }

        private void DrawFunction(int index, float step, IEnumerable<double?> functionPoints, Canvas c, Paint p)
        {
            var color = _functionColors[index];
            var points = functionPoints.ToList();
            p.Color = color;
            p.StrokeWidth = 6;
            var currentX = StartX;
            for (var i = 0; i < points.Count - 1; i++)
            {
                var first = points[i];
                var second = points[i + 1];

                if (first != null && second != null)
                {
                    var firstP = FunctionValue(-first, false);
                    var secondP = FunctionValue(-second, false);
                    if (Math.Abs(firstP - secondP) < 500)
                    {
                        c.DrawLine(FunctionValue(currentX, true), firstP,
                            FunctionValue(currentX + step, true), secondP, p);
                    }
                }
                currentX += step;
            }
        }

        private void DrawFunctions(Canvas c, Paint p)
        {
            var step = 0.05f / Scale;
            var functionValues = _functionManager
                .GetGraphPoints(StartX, StartX + SizeX / _pixelsPerUnit, step);
            var i = 0;
            foreach (var functionPoints in functionValues)
            {
                DrawFunction(i, step, functionPoints, c, p);
                i++;
            }
        }

        private void DrawVerticalLine(float x, int width, Canvas c, Paint p)
        {
            p.StrokeWidth = width;
            p.Color = NetColor;
            c.DrawLine(x, 0, x, SizeY, p);
        }

        private void DrawHorizontalLine(float y, int width, Canvas c, Paint p)
        {
            p.StrokeWidth = width;
            p.Color = NetColor;
            c.DrawLine(0, y, SizeX, y, p);
        }

        private void DrawVerticalNet(float startX, float startUnit, Canvas c, Paint p)
        {
            var currentPoint = startUnit;
            var currentPixels = startX;
            var i = -1;
            while (currentPixels < SizeX)
            {
                i++;
                if (Math.Abs(currentPoint) < 0.01)
                {
                    DrawVerticalLine(currentPixels, 6, c, p);
                }
                if (Math.Abs(currentPoint * 100 % (_scaleString.Frequency * 25)) < 0.1)
                {
                    DrawLabel(currentPixels, _baseY, currentPoint, c, p);
                }
                foreach (var netString in _netStrings.Reverse())
                {
                    if (i < 1 || !(netString.MaxSize < Scale) ||
                        !(Math.Abs((currentPoint * 100) % (netString.Frequency * 25)) < 0.1)) continue;
                    DrawVerticalLine(currentPixels, netString.Width, c, p);
                    break;
                }
                currentPixels += _pixelsPerUnit / 4;
                currentPoint += 0.25f;
            }
        }

        private void DrawHorizontalNet(float startY, float startUnit, Canvas c, Paint p)
        {
            var currentPoint = startUnit;
            var currentPixels = startY;
            var i = -1;
            while (currentPixels < SizeX)
            {
                i++;
                if (Math.Abs(currentPoint) < 0.01)
                {
                    DrawHorizontalLine(currentPixels, 6, c, p);
                }
                if (Math.Abs(currentPoint * 100 % (_scaleString.Frequency * 25)) < 0.1)
                {
                    DrawLabel(_baseX, currentPixels, currentPoint, c, p);
                }
                foreach (var netString in _netStrings.Reverse())
                {
                    if (i < 1 || !(netString.MaxSize < Scale) ||
                        !(Math.Abs(currentPoint * 100 % (netString.Frequency * 25)) < 0.1)) continue;
                    DrawHorizontalLine(currentPixels, netString.Width, c, p);
                    break;
                }
                currentPixels += _pixelsPerUnit / 4;
                currentPoint -= 0.25f;
            }
        }

        private void DrawNet(Canvas c, Paint p)
        {
            var fixedX = Convert.ToInt32(StartX * 100) / 100f;
            var fixedY = Convert.ToInt32(StartY * 100) / 100f;
            var offsetX = (fixedX * 100 % 25) / 100;
            var offsetY = (fixedY * 100 % 25) / 100;
            StartX = fixedX;
            StartY = fixedY;
            SetScale(Scale);
            DrawHorizontalNet(offsetY * _pixelsPerUnit, StartY - offsetY, c, p);
            DrawVerticalNet(-offsetX * _pixelsPerUnit, StartX - offsetX, c, p);
        }

        public float StartX { get; set; }
        public float StartY { get; set; }
        public int SizeX { get; set; }
        public int SizeY { get; set; }
        public float Scale { get; set; }
        public Color NetColor { get; set; }

        public void DrawGraph(Canvas c, Paint p)
        {
            DrawNet(c, p);
            DrawFunctions(c, p);
        }

        public bool AddFunction(IExpression expression, Color color)
        {
            var result = _functionManager.AddFunction(expression);
            if (result)
            {
                _functionColors.Add(color);
            }
            return result;
        }

        public void RemoveFunction(int index)
        {
            var result = _functionManager.RemoveFunction(index);
            if (result)
            {
                _functionColors.RemoveAt(_functionColors.Count-1);
            }
        }

        private void DrawLabel(float positionX, float positionY, float value,
            Canvas c, Paint p)
        {
            p.SetTypeface(_typeface);
            p.Color = Color.Black;
            p.TextSize = 32;
            var text = Math.Round(value, 2).ToString(CultureInfo.InvariantCulture);
            c.DrawText(text, positionX, positionY, p);
        }

        public void ClearGraph()
        {
            _functionColors.Clear();
            _functionManager.ClearAll();
        }

        public void Reset()
        {
            _pixelsPerUnit = DefaultPixels;
            StartX = DefaultStartX;
            StartY = DefaultStartY;
            SetScale(DefaultSize);
        }

        public void SetScale(float scale)
        {
            Scale = scale;
            _pixelsPerUnit = DefaultPixels * scale;
            _baseX = Math.Abs(StartX) * _pixelsPerUnit;
            _baseY = Math.Abs(StartY) * _pixelsPerUnit;
            _scaleString = _netStrings.First(n => n.MaxSize < Scale);
        }

        private float PixelsToPoint(float pixels, bool isX)
        {
            return (pixels - (isX ? _baseX : _baseY)) / _pixelsPerUnit;
        }

        public void ZoomToPoint(float pixelsX, float pixelsY, float scale)
        {
            if (scale < ScaleMin || scale > ScaleMax)
            {
                return;
            }
            var px = PixelsToPoint(pixelsX, true);
            var py = -PixelsToPoint(pixelsY, false);
            SetScale(scale);
            var offsetX = 10 / Scale * (SizeX * 0.5f / 1000);
            var offsetY = 10 / Scale * (SizeY * 0.5f / 1000);
            StartX = px - offsetX;
            StartY = py + offsetY;
            SetScale(scale);
        }

        public void ChangeFunctionAt(int index, IExpression expression)
        {
            _functionManager.ChangeFunctionAt(index, expression);
        }

        public void ZoomToZero(float scale)
        {
            if (scale < ScaleMin || scale > ScaleMax)
            {
                return;
            }
            SetScale(scale);
            var offsetX = 10 / Scale * (SizeX * 0.5f / 1000);
            var offsetY = 10 / Scale * (SizeY * 0.5f / 1000);
            StartX = 0 - offsetX;
            StartY = 0 + offsetY;
            SetScale(scale);
        }

    }
}