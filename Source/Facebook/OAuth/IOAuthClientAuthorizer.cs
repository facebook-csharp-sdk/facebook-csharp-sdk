namespace Facebook.OAuth
{
    using System;

    public interface IOAuthClientAuthorizer
    {
        Uri AuthorizationServerUri { get; }
        string ClientID { get; }
        string ClientSecret { get; }
        Uri RedirectUri { get; }
    }
}