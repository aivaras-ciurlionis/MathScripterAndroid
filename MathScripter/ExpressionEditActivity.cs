using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.InputMethodServices;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Java.Lang;
using MathScripter.Interfaces;
using MathScripter.Models;
using MathScripter.Providers;
using MathScripter.Views;
using Keycode = Android.Views.Keycode;
using Math = System.Math;

namespace MathScripter
{
    [Activity(Label = "ExpressionEditActivity")]
    public class ExpressionEditActivity : Activity, View.IOnTouchListener, KeyboardView.IOnKeyboardActionListener
    {
        private string _expression;
        private ExpressionView _expressionView;
        private EditText _editText;
        private Keyboard _expressionKeyboard;
        private KeyboardView _expressionKeyboardView;
        private string _keyCodes;

        private readonly IEquationKeyResolver _keyResolver =
           App.Container.Resolve(typeof(EquationKeyResolver), "networkDataLoader") as IEquationKeyResolver;

        private void LoadKeyCodes()
        {
            using (var sr = new StreamReader(Assets.Open("keyCodes.json")))
            {
                _keyCodes = sr.ReadToEnd();
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            LoadKeyCodes();
            SetContentView(Resource.Layout.ExpressionEdit);
            _expression = Intent.GetStringExtra("expression") ?? "";
            _expressionView = FindViewById<ExpressionView>(Resource.Id.expressionView);
            _editText = FindViewById<EditText>(Resource.Id.expressionEdit);
            _expressionView.SetNotSolve();
            _expressionView.SetExpression(_expression);
            _expressionView.SetMode(ExpressionViewMode.Expression);
            _editText.Text = _expression;
            _editText.TextChanged += _onExpressionChanged;
            _editText.SetOnTouchListener(this);
            _expressionKeyboard = new Keyboard(this, Resource.Layout.EquationKeyboard);
            _expressionKeyboardView = FindViewById<KeyboardView>(Resource.Id.keyboardview);
            _expressionKeyboardView.Keyboard = _expressionKeyboard;
            _expressionKeyboardView.OnKeyboardActionListener = this;
            _expressionKeyboardView.PreviewEnabled = false;
        }

        protected override void OnPause()
        {
            base.OnPause();
            _expressionView?.Clear();
        }

        public void OpenKeyboard(View v)
        {
            var serv = GetSystemService(InputMethodService) as InputMethodManager;
            serv?.HideSoftInputFromWindow(v.WindowToken, 0);
        }

        private void _onExpressionChanged(object sender, EventArgs args)
        {
            _expression = _editText.Text;
            _expressionView.SetExpression(_expression);
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            v.OnTouchEvent(e);
            var serv = GetSystemService(InputMethodService) as InputMethodManager;
            serv?.HideSoftInputFromWindow(v.WindowToken, 0);
            return true;
        }

        private void AddText(int keyCode)
        {
            var str = _keyResolver.ResolveKey(keyCode, _keyCodes);
            var start = Math.Max(_editText.SelectionStart, 0);
            var end = Math.Max(_editText.SelectionEnd, 0);
            var text = _editText.EditableText;
            text.Replace(Math.Min(start, end), Math.Max(start, end), str, 0, str.Length);
        }

        private void RemoveChar()
        {
            var start = Math.Max(_editText.SelectionStart, 0);
            var end = Math.Max(_editText.SelectionEnd, 0);
            var text = _editText.EditableText;
            text.Delete(Math.Max(start - 1, 0), end);
        }

        private void ClearText()
        {
            _editText.EditableText.Clear();
        }

        public void ResolveExpression()
        {
            var result = new Intent(this, typeof(MainActivity));
            result.PutExtra("expression", _expression);
            SetResult(Result.Ok, result);
            Finish();
        }

        public void OnKey(Keycode primaryCode, Keycode[] keyCodes)
        {
            var code = (int)primaryCode;
            switch (code)
            {
                case 0:
                    ResolveExpression();
                    break;
                case -1:
                    RemoveChar();
                    break;
                case -2:
                    ClearText();
                    break;
                default:
                    AddText(code);
                    break;
            }
        }

        public void OnPress(Keycode primaryCode)
        {
        }

        public void OnRelease(Keycode primaryCode)
        {
        }

        public void OnText(ICharSequence text)
        {
        }

        public void SwipeDown()
        {
        }

        public void SwipeLeft()
        {
        }

        public void SwipeRight()
        {
        }

        public void SwipeUp()
        {
        }
    }
}