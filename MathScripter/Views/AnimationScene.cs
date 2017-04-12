using System.Collections.Generic;
using CocosSharp;
using MathDrawer.Models;

namespace MathScripter.Views
{
    public class AnimationScene : CCScene
    {

        private CCLayer _mainLayer;
        private CCLabel _scoreLabel;

        public AnimationScene(CCWindow window, IEnumerable<DrawableExpression> elements) : base(window)
        {
            _mainLayer = new CCLayer();
            AddChild(_mainLayer);
            foreach (var drawableExpression in elements)
            {
                foreach (var element in drawableExpression.Elements)
                {
                    var l = new CCLabel(element.Text?? "", "LinLibertine_R", element.Size)
                    {
                        PositionX = element.X,
                        PositionY = element.Y,
                        AnchorPoint = CCPoint.AnchorLowerLeft
                    };
                    _mainLayer.AddChild(l);
                    var moveAction = new CCMoveTo(10f, new CCPoint(100, 500));
                    var fadeOutAction = new CCFadeOut(4f);

                    l.RunAction(fadeOutAction);
                }
            }
        }

        public AnimationScene(CCWindow window, CCViewport viewport, CCDirector director = null) : base(window, viewport, director)
        {
        }

        public AnimationScene(CCWindow window, CCDirector director) : base(window, director)
        {
        }

        public AnimationScene(CCWindow window) : base(window)
        {
        }

        public AnimationScene(CCScene scene) : base(scene)
        {
        }

    }
}