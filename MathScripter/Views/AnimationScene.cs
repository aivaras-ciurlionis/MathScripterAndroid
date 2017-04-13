using System.Collections.Generic;
using System.Linq;
using CocosSharp;
using MathDrawer;
using MathDrawer.Interfaces;
using MathDrawer.Models;
using MathExecutor.Interfaces;
using MathExecutor.Models;
using MathExecutor.RuleBinders;
using MathScripter.Models;

namespace MathScripter.Views
{
    public class AnimationScene : CCScene
    {
        private readonly CCLayer _mainLayer;

        private IEnumerable<Step> _steps;
        private IEnumerable<IEnumerable<DrawableExpression>> _drawableSteps;
        private const float Speed = 3f;
        private int _currentStep;

        private readonly IRecursiveRuleMathcer _ruleMatcher =
            App.Container.Resolve(typeof(RecursiveRuleMatcher), "recursiveRuleMatcher") as IRecursiveRuleMathcer;

        private readonly IAnimationStepsDrawer _animationDrawer =
          App.Container.Resolve(typeof(AnimationStepsDrawer), "animationStepsDrawer") as IAnimationStepsDrawer;

        public AnimationScene(CCWindow window, IExpression expression) : base(window)
        {
            _mainLayer = new CCLayer();
            AddChild(_mainLayer);
            LoadSteps(expression);
            Schedule(AnimateSteps, Speed + Speed * 0.2f, (uint)(_drawableSteps.Count() - 1), 0);
        }

        private void AnimateSteps(float time)
        {
            _mainLayer.RemoveAllChildren();
            var actions = new List<AnimationAction>();
            if (_currentStep >= _drawableSteps.Count() - 1)
            {
                return;
            }
            var step1 = _drawableSteps.ElementAt(_currentStep);
            var step2 = _drawableSteps.ElementAt(_currentStep + 1);
            var fitstSteps = step1.Union(new List<DrawableExpression>()).ToList();
            var secondSteps = step2.Union(new List<DrawableExpression>()).ToList();
            var i = 0;
            while (i < fitstSteps.Count)
            {
                var element = fitstSteps[i];
                var moveables = DrawExpression(element);
                var matchingElement = secondSteps.FirstOrDefault(s => s.Id == element.Id);
                if (matchingElement != null)
                {
                    var moveAction = new CCMoveTo(Speed,
                        new CCPoint(matchingElement.Elements[0].X, matchingElement.Elements[0].Y));
                    actions.Add(new AnimationAction { Node = moveables.First(), Action = moveAction });
                    secondSteps.Remove(matchingElement);
                }
                else
                {
                    var fadeAction = new CCFadeOut(Speed * 0.25f);
                    moveables.First().Color = CCColor3B.Red;
                    actions.Add(new AnimationAction { Node = moveables.First(), Action = fadeAction });
                }
                i++;
            }

            foreach (var step in secondSteps)
            {
                var fadeInAction = new CCFadeIn(Speed * 0.5f);
                var moveables = DrawExpression(step);
                moveables.First().Opacity = 0;
                actions.Add(new AnimationAction { Node = moveables.First(), Action = fadeInAction });
            }
            _currentStep += 1;

            foreach (var animationAction in actions)
            {
                animationAction.Node.RunActions(new CCDelayTime(Speed * 0.2f), animationAction.Action);
            }

        }

        private IEnumerable<CCLabel> DrawExpression(DrawableExpression expression)
        {
            var labels = new List<CCLabel>();
            foreach (var element in expression.Elements)
            {
                var l = new CCLabel(element.Text ?? "", "LinLibertine_R", element.Size)
                {
                    PositionX = element.X,
                    PositionY = element.Y,
                    AnchorPoint = CCPoint.AnchorLowerLeft
                };
                _mainLayer.AddChild(l);
                labels.Add(l);
            }
            return labels;
        }

        private void InitStep(IEnumerable<DrawableExpression> step)
        {
            foreach (var drawableExpression in step)
            {
                DrawExpression(drawableExpression);
            }
        }

        private void LoadSteps(IExpression expression)
        {
            _steps = _ruleMatcher.SolveExpression(expression.Clone());
            _drawableSteps = _animationDrawer.GetAnimationSteps(
                _steps,
                null,
                (int)(_mainLayer.VisibleBoundsWorldspace.Center.X),
                (int)(_mainLayer.VisibleBoundsWorldspace.Center.Y),
                (int)_mainLayer.VisibleBoundsWorldspace.MaxX,
                (int)_mainLayer.VisibleBoundsWorldspace.MaxY
            );
        }

    }
}