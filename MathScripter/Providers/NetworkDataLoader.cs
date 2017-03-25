using System.Collections.Generic;
using System.IO;
using Android.Content.Res;
using MathRecognizer.Network;
using MathScripter.Interfaces;

namespace MathScripter.Providers
{
    public class NetworkDataLoader : INetworkDataLoader
    {
        private void LoadFromAssets(AssetManager assets)
        {
            var contents = new List<string>();
            using (var sr = new StreamReader(assets.Open("layer1_weights.txt")))
            {
                contents.Add(sr.ReadToEnd());
            }
            using (var sr = new StreamReader(assets.Open("layer1_biases.txt")))
            {
                contents.Add(sr.ReadToEnd());
            }
            using (var sr = new StreamReader(assets.Open("layer2_weights.txt")))
            {
                contents.Add(sr.ReadToEnd());
            }
            using (var sr = new StreamReader(assets.Open("layer2_biases.txt")))
            {
                contents.Add(sr.ReadToEnd());
            }
            NeuralNetwork.Instance.LoadNetwork(new[] { 4096, 100, 32 }, contents.ToArray());
        }

        public void LoadData(AssetManager assetManager)
        {
            if (!NeuralNetwork.Instance.DataLoaded)
            {
                LoadFromAssets(assetManager);
            }
        }

    }
}