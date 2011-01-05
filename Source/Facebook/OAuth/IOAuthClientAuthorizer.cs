namespace Facebook.OAuth
{
    using System;

    public interface IOAuthClientAuthorizer
    {
        string ClientID { get; }
        string ClientSecret { get; }
        Uri RedirectUri { get; }
    }
}