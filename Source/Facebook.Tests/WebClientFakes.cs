namespace Facebook.Tests
{
    using FakeWebClients;

    public static class WebClientFakes
    {
        internal static IWebClient DownloadAndUploadData(string requestUrl, byte[] returnData)
        {
            return new FakeWebClientForDownloadAndUploadData(returnData);
        }

        internal static IWebClient DownloadAndUploadData(string requestUrl, string returnData)
        {
            return DownloadAndUploadData(requestUrl, System.Text.Encoding.UTF8.GetBytes(returnData));
        }

        internal static IWebClient DownloadAndUploadDataThrowsGraphException(string requestUrl, string jsonResult)
        {
            return new FakeWebClientForDownloadAndUploadDataThrowsGraphException(jsonResult);
        }

        internal static WebExceptionWrapper GetFakeWebException(string json)
        {
            return new FakeWebException(json);
        }
    }
}