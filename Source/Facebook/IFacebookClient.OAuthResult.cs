namespace Facebook
{
    using System;

    public partial interface IFacebookClient
    {
        bool TryParseOAuthCallbackUrl(Uri url, out FacebookOAuthResult facebookOAuthResult);
        FacebookOAuthResult ParseOAuthCallbackUrl(Uri uri);
        Uri GetLoginUrl(object parameters);
        Uri GetLogoutUrl(object parameters);
    }
}
