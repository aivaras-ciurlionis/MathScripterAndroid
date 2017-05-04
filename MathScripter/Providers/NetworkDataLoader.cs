using System.Collections.Generic;
using System.IO;
using System.Linq;
using Android.Content.Res;
using Android.Net;
using Android.OS;
using MathRecognizer.Network;
using MathScripter.Interfaces;

namespace MathScripter.Providers
{
    public class NetworkDataLoader : INetworkDataLoader
    {
        private readonly IDataUpdater _dataUpdater;

        private readonly List<string> _assetNames = new List<string>
        {
            "layer1_weights.txt",
            "layer1_biases.txt",
            "layer2_weights.txt",
            "layer2_biases.txt"
        };

        private readonly string _externalDirectory = Path.Combine(Environment.ExternalStorageDirectory.AbsolutePath,
            "MathScripter");

        public NetworkDataLoader(IDataUpdater dataUpdater)
        {
            _dataUpdater = dataUpdater;
        }

        private bool HasExternalAssets()
        {
            return _assetNames.All(assetName =>
            File.Exists(Path.Combine(_externalDirectory, $"{assetName}_use.txt")));
        }

        private void LoadNetworkVariables(AssetManager assetManager)
        {
            var contents = HasExternalAssets()
                ? LoadFromExternal()
                : LoadFromAssets(assetManager);
            NeuralNetwork.Instance.LoadNetwork(new[] { 4096, 100, 32 }, contents.ToArray());
        }

        private List<string> LoadFromExternal()
        {
            var contents = new List<string>();
            foreach (var assetName in _assetNames)
            {
                using (var sr = new StreamReader(
                    Path.Combine(_externalDirectory, $"{assetName}_use.txt")))
                {
                    contents.Add(sr.ReadToEnd());
                }
            }
            return contents;
        }

        private List<string> LoadFromAssets(AssetManager assets)
        {
            var contents = new List<string>();
            foreach (var assetName in _assetNames)
            {
                using (var sr = new StreamReader(assets.Open(assetName)))
                {
                    contents.Add(sr.ReadToEnd());
                }
            }
            return contents;
        }

        public void LoadData(AssetManager assetManager, ConnectivityManager conection)
        {
            if (!NeuralNetwork.Instance.DataLoaded)
            {
                LoadNetworkVariables(assetManager);
            }
            _dataUpdater.UpdateData(conection);
        }

    }
}