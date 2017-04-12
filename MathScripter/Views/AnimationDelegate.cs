using System.Collections.Generic;
using CocosSharp;
using MathDrawer.Models;

namespace MathScripter.Views
{
    public class AnimationDelegate : CCApplicationDelegate
    {
        private readonly IEnumerable<DrawableExpression> _elements;

        public AnimationDelegate(IEnumerable<DrawableExpression> elements)
        {
            _elements = elements;
        }

        public override void ApplicationDidFinishLaunching(CCApplication application, CCWindow mainWindow)
        {
            application.PreferMultiSampling = false;
            application.ContentRootDirectory = "Content";

            var bounds = mainWindow.WindowSizeInPixels;
            CCScene.SetDefaultDesignResolution(bounds.Width, bounds.Height, CCSceneResolutionPolicy.ShowAll);

            var gameScene = new AnimationScene(mainWindow, _elements);
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