namespace Facebook.OAuth
{
    using System;
    using System.Collections.Generic;

    public interface IOAuthClientAuthorizer
    {
        Uri AuthorizationServerUri { get; }
        string ClientID { get; }
        string ClientSecret { get; }
        Uri RedirectUri { get; }

        Uri GetDesktopLoginUri(IDictionary<string, object> parameters);
        Uri GetDesktopLogoutUri(IDictionary<string, object> parameters);

#if !SILVERLIGHT // silverlight should have only async calls

        // TODO: implement async version of ExchangeAccessTokenForCode
        object ExchangeAccessTokenForCode(string code, IDictionary<string, object> parameters);

#endif

    }
}