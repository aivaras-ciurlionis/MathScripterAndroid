using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MathRecognizer.Interfaces;

namespace MathRecognizer.Network
{
    public class NetworkWrapper : INeuralNetwork
    {
        private readonly IInputNormalizer _inputNormalizer;

        public NetworkWrapper(IInputNormalizer inputNormalizer)
        {
            _inputNormalizer = inputNormalizer;
        }

        public int GetPrediction(byte[] input)
        {
            if (!NeuralNetwork.Instance.DataLoaded) return -1;
            var normalizedInputs = _inputNormalizer.NormalizeInput(input);
            return NeuralNetwork.Instance.GetNetworkOutput(normalizedInputs);
        }
    }
}