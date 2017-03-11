using System;
using Android.App;
using Android.Runtime;
using MathRecognizer;
using MathRecognizer.EquationBuilding;
using MathRecognizer.ImageDecoding;
using MathRecognizer.ImageProcessing;
using MathRecognizer.Interfaces;
using MathRecognizer.Network;
using MathRecognizer.Segmentation;
using MathRecognizer.SegmentsRecognition;
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
        }
    }
}