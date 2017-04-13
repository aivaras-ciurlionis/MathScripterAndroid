using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Graphics;
using Android.OS;
using Microsoft.Xna.Framework;
using CocosSharp;
using MathDrawer;
using MathDrawer.Interfaces;
using MathDrawer.Models;
using MathExecutor.Expressions;
using MathExecutor.Interfaces;
using MathExecutor.Interpreter;
using MathExecutor.Models;
using MathScripter.Views;

namespace MathScripter
{
    [Activity(Label = "Animation")]
    public class AnimationActivity : AndroidGameActivity
    {
        private readonly IInterpreter _interpreter =
           App.Container.Resolve(typeof(Interpreter), "interpreter") as IInterpreter;

        private readonly IBaseDrawer _baseDrawer =
           App.Container.Resolve(typeof(BaseDrawer), "interpreter") as IBaseDrawer;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var e = Intent.GetStringExtra("expression");
            var expression = _interpreter.GetExpression(e);
            var application = new CCApplication {ApplicationDelegate = new AnimationDelegate(expression) };
            SetContentView(application.AndroidContentView);
            application.StartGame();
        }
    }
}