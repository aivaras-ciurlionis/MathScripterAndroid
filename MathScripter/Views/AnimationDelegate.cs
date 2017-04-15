using System.Collections.Generic;
using CocosSharp;
using MathDrawer.Models;
using MathExecutor.Interfaces;

namespace MathScripter.Views
{
    public class AnimationDelegate : CCApplicationDelegate
    {
        private readonly IExpression _expression;

        public AnimationDelegate(IExpression elements)
        {
            _expression = elements;
        }

        public override void ApplicationDidFinishLaunching(CCApplication application, CCWindow mainWindow)
        {
            application.PreferMultiSampling = false;
            application.ContentRootDirectory = "Content";
            application.ContentSearchPaths.Add("Fonts");

            var bounds = mainWindow.WindowSizeInPixels;
            CCScene.SetDefaultDesignResolution(bounds.Width, bounds.Height, CCSceneResolutionPolicy.ShowAll);

            var gameScene = new AnimationScene(mainWindow, _expression);
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