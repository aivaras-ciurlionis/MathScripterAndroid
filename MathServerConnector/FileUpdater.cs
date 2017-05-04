using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathServerConnector.Interfaces;
using Newtonsoft.Json.Linq;

namespace MathServerConnector
{
    public class FileUpdater : IFileUpdater
    {
        private readonly IMathWebClient _client;

        private const string VersionEndpoint = "/api/downloadables/is-newest";
        private const string MathDirectory = "MathScripter";

        public FileUpdater(IMathWebClient client)
        {
            _client = client;
        }

        private void SaveVersion(int version, string versionFile)
        {
            using (var streamWriter = new StreamWriter(versionFile))
            {
                streamWriter.Write(version);
            }
        }

        private bool NeedsUpdating(string versionFile)
        {
            int versionNumber;
            try
            {
                using (var streamReader = new StreamReader(versionFile))
                {
                    var versionContent = streamReader.ReadToEnd();
                    versionNumber = int.Parse(versionContent);
                }
            }
            catch (IOException)
            {
                return true;
            }
            try
            {
                var versionResponse = _client.GetData($"{VersionEndpoint}?version={versionNumber}");
                JToken token = JObject.Parse(versionResponse);
                var newestVersion = (bool)token.SelectToken("result");
                return newestVersion;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private IEnumerable<string> GetFileUrls()
        {
            var content = _client.GetData("/api/downloadables");
            var fileList = JArray.Parse(content);
            return fileList.ToObject<List<string>>();
        }

        private void RenameFilesToUsable(IEnumerable<string> paths)
        {
            foreach (var path in paths)
            {
                var destination = $"{path}_use.txt";
                if (File.Exists(destination))
                {
                    File.Delete(destination);
                }
                File.Move(path, destination);
            }
        }

        private static void RemoveUsableFiles(string destinationDirectory)
        {
            var directoryFiles = Directory.GetFiles(destinationDirectory);
            var usableFiles = directoryFiles.Where(d => d.Contains("use_"));
            foreach (var usableFile in usableFiles)
            {
                if (File.Exists(usableFile))
                {
                    File.Delete(usableFile);
                }
            }
        }

        private int GetCurrentVersion()
        {
            try
            {
                var content = _client.GetData("/api/downloadables/newest-version");
                JToken versionObject = JObject.Parse(content);
                return (int)versionObject.SelectToken("version");
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public void UpdateNetworkFiles(string destinationDirectory)
        {
            var fullDirectory = Path.Combine(destinationDirectory, MathDirectory);
            if (!Directory.Exists(fullDirectory))
            {
                Directory.CreateDirectory(fullDirectory);
            }
            if (!NeedsUpdating(Path.Combine(fullDirectory, "version.txt")))
            {
                Console.WriteLine("No need to update");
                return;
            }
            try
            {
                Console.WriteLine("Getting file names");
                var fileUrls = GetFileUrls();
                var fileNames = new List<string>();
                Console.WriteLine("Downloading files");
                foreach (var fileUrl in fileUrls)
                {
                    var fileName = fileUrl.Split('/').Last();
                    _client.DownloadFile(fileUrl, Path.Combine(fullDirectory, fileName));
                    fileNames.Add(Path.Combine(fullDirectory, fileName));
                }
                RemoveUsableFiles(fullDirectory);
                RenameFilesToUsable(fileNames);
                var version = GetCurrentVersion();
                SaveVersion(version, Path.Combine(fullDirectory, "version.txt"));
                Console.WriteLine("Finished!");
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}