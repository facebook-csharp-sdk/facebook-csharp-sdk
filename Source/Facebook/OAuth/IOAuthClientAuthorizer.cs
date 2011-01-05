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

        Uri GetLoginUri(IDictionary<string, object> parameters);
    }
}