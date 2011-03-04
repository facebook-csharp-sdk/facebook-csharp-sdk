namespace Facebook.Tests
{
    using FakeWebClients;

    public static class WebClientFakes
    {
        internal static IWebClient DownloadData(string requestUrl, byte[] returnData)
        {
            return new FakeWebClientForDownloadData(returnData);
        }

        internal static IWebClient DownloadData(string requestUrl, string returnData)
        {
            return DownloadData(requestUrl, System.Text.Encoding.UTF8.GetBytes(returnData));
        }
    }
}