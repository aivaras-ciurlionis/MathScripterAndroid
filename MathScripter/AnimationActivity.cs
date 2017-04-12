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
            var expression = _interpreter.GetExpression("(2+3+4)/(25*x)+17^2");
            if (!(expression is RootExpression))
            {
                expression = new RootExpression(expression, null);
            }
            var el = _baseDrawer.DrawExpression(expression,
                new TextParameters
                {
                    Size = 72,
                    Typeface = Typeface.CreateFromAsset(Assets, "LinLibertine_R.ttf")
                },
                new EquationBounds
                {
                    X = 500,
                    Y = 200,
                    Width = 500,
                    Height = 500
                }, 
                250
             );
            var application = new CCApplication {ApplicationDelegate = new AnimationDelegate(el) };
            SetContentView(application.AndroidContentView);
            application.StartGame();
        }
    }
}