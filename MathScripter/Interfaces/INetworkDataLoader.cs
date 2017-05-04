using Android.Content.Res;
using Android.Net;

namespace MathScripter.Interfaces
{
    public interface INetworkDataLoader
    {
        void LoadData(AssetManager assetManager, ConnectivityManager conection);
    }
}