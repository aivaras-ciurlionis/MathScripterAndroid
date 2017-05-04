using System;
using System.Net;
using MathServerConnector.Interfaces;

namespace MathServerConnector
{
    public class MathWebClient : IMathWebClient
    {
        private const string MathUrl = "http://webapplication120170502072605.azurewebsites.net";
        
        public string GetData(string endpoint)
        {
            var client = new WebClient();
            var result = client.DownloadString(MathUrl + endpoint);
            return result;
        }

        public string PostData(string endpoint, string data)
        {
            var client = new WebClient();
            client.Headers.Add("Content-Type", "application/json");
            var result = client.UploadString(MathUrl + endpoint, data);
            return result;
        }

        public void DownloadFile(string endpoint, string destination)
        {
            var client = new WebClient();
            client.DownloadFile(endpoint, destination);
        }
    }
}