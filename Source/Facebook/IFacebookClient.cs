
namespace Facebook
{
    using System;

    public partial interface IFacebookClient
    {
        string AccessToken { get; set; }
        string AppId { get; set; }
        string AppSecret { get; set; }
        bool IsSecureConnection { get; set; }
        bool UseFacebookBeta { get; set; }
        Func<object, string> SerializeJson { get; set; }
        Func<string, Type, object> DeserializeJson { get; set; }
        Func<Uri, HttpWebRequestWrapper> HttpWebRequestFactory { get; set; }
#if false
		static void SetDefaultJsonSerializers(Func<object, string> jsonSerializer, Func<string, Type, object> jsonDeserializer)
		static void SetDefaultHttpWebRequestFactory(Func<Uri, HttpWebRequestWrapper> httpWebRequestFactory)
#endif
    }
}
