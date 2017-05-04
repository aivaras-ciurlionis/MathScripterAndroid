using System.Threading;
using Android.Net;
using MathScripter.Interfaces;
using MathServerConnector.Interfaces;
using Environment = Android.OS.Environment;
namespace MathScripter.Providers
{
    public class DataUpdater : IDataUpdater
    {
        private readonly IFileUpdater _fileUpdater;

        public DataUpdater(IFileUpdater fileUpdater)
        {
            _fileUpdater = fileUpdater;
        }

        public void UpdateData(ConnectivityManager conection)
        {
            var networkInfo = conection.ActiveNetworkInfo;
            if (networkInfo == null ||
                !networkInfo.IsConnected ||
                networkInfo.Type != ConnectivityType.Wifi)
            {
                return;
            }
            var directory = Environment.ExternalStorageDirectory.AbsolutePath;
            ThreadPool.QueueUserWorkItem(o => _fileUpdater.UpdateNetworkFiles(directory));
        }
    }
}