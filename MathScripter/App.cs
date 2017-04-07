using System;
using Android.App;
using Android.Runtime;
using MathDrawer;
using MathDrawer.Interfaces;
using MathExecutor.Expressions;
using MathExecutor.Helpers;
using MathExecutor.Interfaces;
using MathExecutor.Interpreter;
using MathExecutor.Parser;
using MathExecutor.RuleBinders;
using MathRecognizer;
using MathRecognizer.EquationBuilding;
using MathRecognizer.ImageDecoding;
using MathRecognizer.ImageProcessing;
using MathRecognizer.Interfaces;
using MathRecognizer.Network;
using MathRecognizer.Segmentation;
using MathRecognizer.SegmentsRecognition;
using MathScripter.Interfaces;
using MathScripter.Providers;
using Microsoft.Practices.Unity;

namespace MathScripter
{
    [Application]
    public class App : Application
    {
        public App(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        {
        }

        public static UnityContainer Container { get; set; }

        public override void OnCreate()
        {
            Initialize();
            base.OnCreate();
        }

        public static void Initialize()
        {
            Container = new UnityContainer();

            Container.RegisterType<INetworkDataLoader, NetworkDataLoader>();
            Container.RegisterType<IEquationKeyResolver, EquationKeyResolver>();

            Container.RegisterType<IRecognizer, Recognizer>();
            Container.RegisterType<ICenterOfMassComputor, CenterOfMassComputor>();
            Container.RegisterType<IImageDecoder, ImageDecoder>();
            Container.RegisterType<IImageMover, ImageMover>();
            Container.RegisterType<IPixelsToImageConverter, PixelsToImageConverter>();
            Container.RegisterType<ISegmentator, Segmentator>();
            Container.RegisterType<ISegmentsProcessor, SegmentsProcessor>();
            Container.RegisterType<ISegmentsResizer, SegmentsResizer>();
            Container.RegisterType<INeuralNetwork, NetworkWrapper>();
            Container.RegisterType<IInputNormalizer, InputNormalizer>();
            Container.RegisterType<IIndexMapper, IndexMapper>();
            Container.RegisterType<IRatioResizer, RatioResizer>();
            Container.RegisterType<IEquationsBuilder, EquationsBuilder>();
            Container.RegisterType<IEquationBuilder, EquationBuilder>();
            Container.RegisterType<IBlockBuilder, BlockBuilder>();
            Container.RegisterType<ISegmentsSplitter, SegmentsSplitter>();
            Container.RegisterType<IRectangleDistanceFinder, RectangleDistanceFinder>();
            Container.RegisterType<IRectangleIntersectionFinder, RectangleIntersectionFinder>();
            Container.RegisterType<IMinusRowSeparator, MinusRowSeparator>();
            Container.RegisterType<ISegmentBuilder, SegmentBuilder>();
            Container.RegisterType<IEquationStripper, EquationStripper>();
            Container.RegisterType<ICharacterFixer, CharacterFixer>();
            Container.RegisterType<IEqualitySignFinder, EqualitySignFinder>();

            Container.RegisterType<IMonomialResolver, MonomialResolver>();
            Container.RegisterType<IExpressionFactory, ExpressionFactory>();
            Container.RegisterType<IMinOperationFinder, MinOperationFinder>();
            Container.RegisterType<IExpressionCreator, ExpressionCreator>();
            Container.RegisterType<ISymbolTypeChecker, SymbolTypeChecker>();
            Container.RegisterType<ITokenCreator, TokenCreator>();
            Container.RegisterType<ITokenFixer, TokenFixer>();
            Container.RegisterType<ITokenParser, TokenParser>();
            Container.RegisterType<IParser, Parser>();
            Container.RegisterType<IInterpreter, Interpreter>();
            Container.RegisterType<IExpressionFlatener, ExpressionFlatener>();
            Container.RegisterType<IOtherExpressionAdder, OhterExpressionAdder>();
            Container.RegisterType<IElementsChanger, ElementsChanger>();
            Container.RegisterType<IFinalResultChecker, FinalResultChecker>();
            Container.RegisterType<IRecursiveRuleMathcer, RecursiveRuleMatcher>();
            Container.RegisterType<IMultiRuleChecker, MultiRuleChecher>();
            Container.RegisterType<IParentChecker, ParentChecker>();
            Container.RegisterType<ISequentialRuleMatcher, SequentialRuleMatcher>();

            Container.RegisterType<IBaseDrawer, BaseDrawer>();
            Container.RegisterType<IDrawerFactory, DrawerFactory>();
            Container.RegisterType<IElementsDrawer, ElementsDrawer>();
            Container.RegisterType<IStepsDrawer, StepsDrawer>();
            Container.RegisterType<IExpressionDrawer, ExpressionDrawer>();
        }
    }
}