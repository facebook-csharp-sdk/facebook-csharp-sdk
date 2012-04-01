namespace Facebook
{
    public partial interface IFacebookClient
    {
		bool TryParseSignedRequest(string appSecret, string signedRequestValue, out object signedRequest);
		bool TryParseSignedRequest(string signedRequestValue, out object signedRequest);
		object ParseSignedRequest(string appSecret, string signedRequestValue);
		object ParseSignedRequest(string signedRequestValue);
    }
}
