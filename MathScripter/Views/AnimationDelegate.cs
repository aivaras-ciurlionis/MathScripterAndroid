using System.Collections.Generic;
using Android.App;
using CocosSharp;
using MathDrawer.Models;
using MathExecutor.Interfaces;

namespace MathScripter.Views
{
    public class AnimationDelegate : CCApplicationDelegate
    {
        private readonly IExpression _expression;
        private readonly Activity _launcher;

        public AnimationDelegate(IExpression elements, Activity launcher)
        {
            _expression = elements;
            _launcher = launcher;
        }

        public override void ApplicationDidFinishLaunching(CCApplication application, CCWindow mainWindow)
        {
            application.PreferMultiSampling = false;
            application.ContentRootDirectory = "Content";
            application.ContentSearchPaths.Add("Fonts");

            var bounds = mainWindow.WindowSizeInPixels;
            CCScene.SetDefaultDesignResolution(bounds.Width, bounds.Height, CCSceneResolutionPolicy.ShowAll);

            var gameScene = new AnimationScene(mainWindow, _expression, application, _launcher);
            mainWindow.RunWithScene(gameScene);
        }

        public override void ApplicationDidEnterBackground(CCApplication application)
        {
        }

        public override void ApplicationWillEnterForeground(CCApplication application)
        {
        }
    }
}