using System;
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
using MathScripter.Providers;

namespace MathScripter.Views
{
    public class AnimationScene : CCScene
    {
        private readonly CCLayer _mainLayer;

        private IEnumerable<Step> _steps;
        private IEnumerable<IEnumerable<DrawableExpression>> _drawableSteps;
        private const float Speed = 2f;
        private int _currentStep;

        private readonly IRecursiveRuleMathcer _ruleMatcher =
            App.Container.Resolve(typeof(RecursiveRuleMatcher), "recursiveRuleMatcher") as IRecursiveRuleMathcer;

        private readonly IAnimationStepsDrawer _animationDrawer =
          App.Container.Resolve(typeof(AnimationStepsDrawer), "animationStepsDrawer") as IAnimationStepsDrawer;




        public AnimationScene(CCWindow window, IExpression expression) : base(window)
        {
            _mainLayer = new CCLayer();
            AddChild(_mainLayer);
            DrawBackground();
            LoadSteps(expression);
            InitStep(_drawableSteps.First());
            Schedule(AnimateSteps, Speed + Speed * 0.2f, (uint) (_drawableSteps.Count() - 2), Speed);
        }

        private void DrawBackground()
        {
            var drawNode = new CCDrawNode
            {
                PositionX = 0,
                PositionY = 0
            };
            _mainLayer.AddChild(drawNode);
            var shape = new CCRect(
             0, 0, _mainLayer.VisibleBoundsWorldspace.MaxX, _mainLayer.VisibleBoundsWorldspace.MaxY);
            drawNode.DrawRect(shape, CCColor4B.White);
        }

        private IEnumerable<AnimationAction> GetMoveableActions(IEnumerable<CCNode> labels,
            DrawableExpression expression, DrawableElement triggerElement = null)
        {
            var fractionDrawer = new CocosFractionDrawer();
            var rootDrawer = new CocosRootDrawer();
            var actions = new List<AnimationAction>();
            var ccLabels = labels as IList<CCNode> ?? labels.ToList();
            for (var i = 0; i < ccLabels.Count; i++)
            {
                if (i >= expression.Elements.Count)
                {
                    var fade = new CCFadeOut(Speed);
                    actions.Add(new AnimationAction { Node = ccLabels[i], Action = fade });
                    continue;
                }
                if (triggerElement != null &&
                    triggerElement.Type == DrawableType.Division &&
                   Math.Abs(triggerElement.Width - expression.Elements[i].Width) > 15)
                {
                    var fade = new CCScaleTo(Speed*0.1f, ccLabels[i].ScaleX, 0);
                    actions.Add(new AnimationAction { Node = ccLabels[i], Action = fade });
                    var n = fractionDrawer.DrawFraction(expression.Elements[i], _mainLayer.VisibleBoundsWorldspace.MaxY);
                    var startX = n.ScaleX;
                    var startY = n.ScaleY;
                    n.ScaleY = 0;
                    _mainLayer.AddChild(n);
                    actions.Add(new AnimationAction { Node = n, Action = new CCScaleTo(Speed * 0.1f, startX, startY) });
                    continue;
                }

                if (triggerElement != null &&
                    triggerElement.Type == DrawableType.Root &&
                   Math.Abs(triggerElement.Width - expression.Elements[i].Width) > 15)
                {
                    var fade = new CCScaleTo(Speed * 0.1f, ccLabels[i].ScaleX, 0);
                    actions.Add(new AnimationAction { Node = ccLabels[i], Action = fade });
                    var n = rootDrawer.DrawRoot(expression.Elements[i], _mainLayer.VisibleBoundsWorldspace.MaxY);
                    var startX = n.ScaleX;
                    var startY = n.ScaleY;
                    n.ScaleY = 0;
                    _mainLayer.AddChild(n);
                    actions.Add(new AnimationAction { Node = n, Action = new CCScaleTo(Speed * 0.1f, startX, startY) });
                    continue;
                }

                var element = expression.Elements[i];
                var offset = triggerElement != null && triggerElement.Type != DrawableType.Symbolic
                    ? triggerElement.Type == DrawableType.Division ? element.Height * 3 : element.Size * 0.075f * 2.5f
                    : 0;
                    var moveAction = new CCMoveTo(Speed,
                        new CCPoint(element.X, _mainLayer.VisibleBoundsWorldspace.MaxY - element.Y + offset));
                    actions.Add(new AnimationAction { Node = ccLabels[i], Action = moveAction });
            }
            return actions;
        }

        private IEnumerable<AnimationAction> GetActionsForElements(CCFiniteTimeAction action,
            IEnumerable<CCNode> labels, bool mark = false, bool hide = false)
        {
            var actions = new List<AnimationAction>();
            foreach (var label in labels)
            {
                if (mark)
                {
                    label.Color = CCColor3B.Red;
                }
                if (hide)
                {
                    label.Opacity = 0;
                }
                actions.Add(new AnimationAction { Node = label, Action = action });
            }
            return actions;
        }

        private void AnimateSteps(float time)
        {
            if (_currentStep >= _drawableSteps.Count() - 1)
            {
                return;
            }
            var actions = new List<AnimationAction>();
            _mainLayer.RemoveAllChildren();
            DrawBackground();
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
                    actions.AddRange(GetMoveableActions(moveables, matchingElement, element.Elements.First()));
                    secondSteps.Remove(matchingElement);
                }
                else
                {
                    var fadeAction = GetFadeAction(false, element.Elements.First().Type,
                    moveables.First().ScaleX, 0);
                    actions.AddRange(GetActionsForElements(fadeAction, moveables, true));
                }
                i++;
            }

            foreach (var step in secondSteps)
            {
                var moveables = DrawExpression(step);
                var fadeInAction = GetFadeAction(true, step.Elements.First().Type,
                    moveables.First().ScaleX, moveables.First().ScaleY, moveables.First());
                actions.AddRange(GetActionsForElements(fadeInAction, moveables, false, true));
            }
            _currentStep += 1;

            foreach (var animationAction in actions)
            {
                animationAction.Node.RunActions(new CCDelayTime(Speed * 0.2f), animationAction.Action);
            }

        }

        private CCFiniteTimeAction GetFadeAction(bool fadeIn, DrawableType type,
            float scaleX, float scaleY, CCNode node = null)
        {
            CCFiniteTimeAction fadeAction;
            if (fadeIn)
            {
                fadeAction = new CCFadeIn(Speed * 0.5f);
            }
            else
            {
                fadeAction = new CCFadeOut(Speed * 0.5f);
            }

            if (type == DrawableType.Symbolic) return fadeAction;
            if (node != null && fadeIn)
            {
                node.ScaleY = 0;
            }
            fadeAction = new CCScaleTo(Speed * 0.2f, scaleX,
                scaleY);
            return fadeAction;
        }

        private IEnumerable<CCNode> DrawExpression(DrawableExpression expression)
        {
            var labels = new List<CCNode>();
            var fractionDrawer = new CocosFractionDrawer();
            var textDrawer = new CocosTextDrawer();
            var rootDrawer = new CocosRootDrawer();
            foreach (var element in expression.Elements)
            {
                CCNode node = null;
                switch (element.Type)
                {
                    case DrawableType.Symbolic:
                        node = textDrawer.DrawText(element, _mainLayer.VisibleBoundsWorldspace.MaxY);
                        break;
                    case DrawableType.Division:
                        node = fractionDrawer.DrawFraction(element, _mainLayer.VisibleBoundsWorldspace.MaxY);
                        break;
                    case DrawableType.Root:
                        node = rootDrawer.DrawRoot(element, _mainLayer.VisibleBoundsWorldspace.MaxY);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                _mainLayer.AddChild(node);
                labels.Add(node);
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