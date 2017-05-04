namespace MathServerConnector.Interfaces
{
    public interface IMathWebClient
    {
        string GetData(string endpoint);
        string PostData(string endpoint, string data);
        void DownloadFile(string endpoint, string destination);
    }
}