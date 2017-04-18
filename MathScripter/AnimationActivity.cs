using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using CocosSharp;
using MathExecutor.Interfaces;
using MathExecutor.Interpreter;
using MathScripter.Views;

namespace MathScripter
{
    [Activity(Label = "Animation")]
    public class AnimationActivity : Activity
    {
        private readonly IInterpreter _interpreter =
           App.Container.Resolve(typeof(Interpreter), "interpreter") as IInterpreter;

        private IExpression _expression;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Animation);
            var e = Intent.GetStringExtra("expression");
            _expression = _interpreter.GetExpression(e);
            var gameView = (CCGameView) FindViewById(Resource.Id.AnimationView);
            gameView.ViewCreated += LoadGame;
        }

        private void LoadGame(object sender, EventArgs e)
        {
            var contentSearchPaths = new List<string> {"Fonts"};
            var gameView = sender as CCGameView;
            if (gameView == null) return;
            // Set world dimensions
            gameView.ContentManager.SearchPaths = contentSearchPaths;
            gameView.DesignResolution = new CCSizeI(gameView.Width, gameView.Height);
            var gameScene = new AnimationScene(_expression, this, gameView);
            gameView.RunWithScene(gameScene);
        }

    }
}