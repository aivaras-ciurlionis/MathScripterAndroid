using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra.Double;
using MathRecognizer.Models;

namespace MathRecognizer.Network
{
    public class NeuralNetwork
    {
        private static NeuralNetwork _network;
        public bool DataLoaded { get; set; }
        private NeuralNetwork()
        {
            DataLoaded = false;
        }

        private readonly IList<NetworkLayer> _layers = new List<NetworkLayer>();

        public static NeuralNetwork Instance => _network ?? (_network = new NeuralNetwork());

        public void LoadNetwork(int[] sizes, string[] resourceContents)
        {
            for (var i = 0; i < sizes.Length - 1; i++)
            {
                _layers.Add(new NetworkLayer(sizes[i], sizes[i + 1], resourceContents[i * 2], resourceContents[i * 2 + 1]));
            }
            DataLoaded = true;
        }

        public int GetNetworkOutput(double[] input)
        {
            var feedVector = DenseVector.OfArray(input);
            var j = 0;
            foreach (var networkLayer in _layers)
            {
                j++;
                var layerOutput = feedVector * networkLayer.Weights + networkLayer.Biases;
                feedVector = layerOutput as DenseVector;
                if (j >= _layers.Count) continue;
                for (var i = 0; i < feedVector.Count; i++)
                {
                    feedVector[i] = Math.Max(feedVector[i], 0);
                }
            }
            return feedVector.MaximumIndex() + 1;
        }

    }
}