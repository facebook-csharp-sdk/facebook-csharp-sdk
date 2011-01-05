namespace Facebook.OAuth
{
    using System;

    public interface IOAuthClientAuthorizer
    {
        string ClientID { get; }
        string ClientSecret { get; set; }
        Uri RedirectUri { get; set; }
    }
}