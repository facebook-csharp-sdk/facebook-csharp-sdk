namespace Facebook
{
    using System;

    internal class WebClientStateContainer
    {
        public object UserState { get; set; }
        public HttpMethod Method { get; set; }
        public Uri RequestUri { get; set; }
        public bool IsBatchRequest { get; set; }
    }
}
