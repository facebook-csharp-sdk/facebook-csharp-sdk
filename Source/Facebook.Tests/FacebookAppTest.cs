namespace Facebook
{
    using System.Collections.Generic;
    using System.Dynamic;
    using System.IO;
    using Xunit;

    public class FacebookAppTest
    {
        [Fact]
        public void BuildMediaObjectPostData_Tests()
        {
            var assmebly = System.Reflection.Assembly.GetExecutingAssembly();
            var stream = assmebly.GetManifestResourceStream("Facebook.Tests.monkey.jpg");
            byte[] photo = new byte[stream.Length];
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                photo = ms.ToArray();
            }

            dynamic parameters = new ExpandoObject();
            parameters.message = "This is a test photo of a monkey that has been uploaded " +
                                 "by the Facebook C# SDK (http://facebooksdk.codeplex.com)" +
                                 "using the Graph API";
            var mediaObject = new FacebookMediaObject
            {
                FileName = "monkey.jpg",
                ContentType = "image/jpeg",
            };
            mediaObject.SetValue(photo);
            parameters.source = mediaObject;

            string boundary = "8cd62a36054bd4c";
            byte[] actual = FacebookApp.BuildMediaObjectPostData(parameters, boundary);

            Assert.Equal(127231, actual.Length);
        }

        [Fact(DisplayName = "ParseSignedRequest test")]
        public void ParseSignedRequest_Test()
        {
            var signed_request = "Iin8a5nlQOHhlvHu_4lNhKDDvut6s__fm6-jJytkHis.eyJhbGdvcml0aG0iOiJITUFDLVNIQTI1NiIsImV4cGlyZXMiOjEyODI5Mjg0MDAsIm9hdXRoX3Rva2VuIjoiMTIwNjI1NzAxMzAxMzQ3fDIuSTNXUEZuXzlrSmVnUU5EZjVLX0kyZ19fLjM2MDAuMTI4MjkyODQwMC0xNDgxMjAxN3xxcmZpT2VwYnY0ZnN3Y2RZdFJXZkFOb3I5YlEuIiwidXNlcl9pZCI6IjE0ODEyMDE3In0";
            var appSecret = "543690fae0cd186965412ac4a49548b5";
            var signedRequest = FacebookSignedRequest.Parse(appSecret, signed_request);

            Assert.Equal("120625701301347|2.I3WPFn_9kJegQNDf5K_I2g__.3600.1282928400-14812017|qrfiOepbv4fswcdYtRWfANor9bQ.", signedRequest.AccessToken);
        }

        [Fact(DisplayName = "ParseUrlParameters: Given a url host is facebook graph without querystring and parameter is empty Then return path does not start with forward slash")]
        public void ParseUrlParameters_GivenAUrlHostIsFacebookGraphWithoutQuerystringAndParameterIsEmpty_ThenReturnPathDoesNotStartWithForwardSlash()
        {
            string url = "http://graph.facebook.com/me/likes";
            var parameters = new Dictionary<string, object>();

            var path = FacebookAppBase.ParseQueryParametersToDictionary(url, parameters);

            Assert.NotEqual('/', path[0]);
        }

        [Fact(DisplayName = "ParseUrlParameters: Given a url host is facebook graph with querystring and parameter is empty Then the return path does not start with forward slash")]
        public void ParseUrlParameters_GivenAUrlHostIsFacebookGraphWithQuerystringAndParameterIsEmpty_ThenTheReturnPathDoesNotStartWithForwardSlash()
        {
            string urlWithQueryString = "http://graph.facebook.com/me/likes?limit=3&offset=2";

            var parameters = new Dictionary<string, object>();

            var path = FacebookAppBase.ParseQueryParametersToDictionary(urlWithQueryString, parameters);

            Assert.NotEqual('/', path[0]);
        }

        [Fact(DisplayName = "ParseUrlParameters: Given a url host is facebook graph without querystring and parameter is empty Then return path equals path without uri host and doesnt start with forward slash")]
        public void ParseUrlParameters_GivenAUrlHostIsFacebookGraphWithoutQuerystringAndParameterIsEmpty_ThenReturnPathEqualsPathWithoutUriHostAndDoesntStartWithForwardSlash()
        {
            string url = "http://graph.facebook.com/me/likes";
            string originalPathWithoutForwardSlash = "me/likes";
            var parameters = new Dictionary<string, object>();

            var path = FacebookAppBase.ParseQueryParametersToDictionary(url, parameters);

            Assert.Equal(originalPathWithoutForwardSlash, path);
        }

        [Fact(DisplayName = "ParseUrlParameters: Given a url host is facebook graph with querystring and parameter is empty Then the return path equals path without uri host and does not start with forward slash")]
        public void ParseUrlParameters_GivenAUrlHostIsFacebookGraphWithQuerystringAndParameterIsEmpty_ThenTheReturnPathEqualsPathWithoutUriHostAndDoesNotStartWithForwardSlash()
        {
            string urlWithQueryString = "http://graph.facebook.com/me/likes?limit=3&offset=2";
            string originalPathWithoutForwardSlashAndWithoutQueryString = "me/likes";
            var parameters = new Dictionary<string, object>();

            var path = FacebookAppBase.ParseQueryParametersToDictionary(urlWithQueryString, parameters);

            Assert.Equal(originalPathWithoutForwardSlashAndWithoutQueryString, path);
        }

        [Fact(DisplayName = "ParseUrlParameters: Given a url host is facebook graph without querystring and parameter is empty Then count of parameter is 0")]
        public void ParseUrlParameters_GivenAUrlHostIsFacebookGraphWithoutQuerystringAndParameterIsEmpty_ThenCountOfParameterIs0()
        {
            string url = "http://graph.facebook.com/me/likes";
            var parameters = new Dictionary<string, object>();

            FacebookAppBase.ParseQueryParametersToDictionary(url, parameters);

            Assert.Equal(0, parameters.Count);
        }

        [Fact(DisplayName = "ParseUrlParameters: Given a url host is facebook graph with querystring and parameter is empty Then the count of parameter is equal to the count of querystring")]
        public void ParseUrlParameters_GivenAUrlHostIsFacebookGraphWithQuerystringAndParameterIsEmpty_ThenTheCountOfParameterIsEqualToTheCountOfQuerystring()
        {
            string urlWithQueryString = "http://graph.facebook.com/me/likes?limit=3&offset=2";
            var parameters = new Dictionary<string, object>();

            FacebookAppBase.ParseQueryParametersToDictionary(urlWithQueryString, parameters);

            Assert.Equal(2, parameters.Count);
        }

        [Fact(DisplayName = "ParseUrlParameters: Given a path as empty string and empty parameters Then count of parameters equals 0")]
        public void ParseUrlParameters_GivenAPathAsEmptyStringAndEmptyParameters_ThenCountOfParametersEquals0()
        {
            string path = string.Empty;
            var parameters = new Dictionary<string, object>();

            FacebookAppBase.ParseQueryParametersToDictionary(path, parameters);

            Assert.Equal(0, parameters.Count);
        }

        [Fact(DisplayName = "ParseUrlParameters: Given a path without querystring and empty parameters Then count of parameters equals 0")]
        public void ParseUrlParameters_GivenAPathWithoutQuerystringAndEmptyParameters_ThenCountOfParametersEquals0()
        {
            var path = "/me/likes";
            var parameters = new Dictionary<string, object>();

            FacebookAppBase.ParseQueryParametersToDictionary(path, parameters);

            Assert.Equal(0, parameters.Count);
        }

        [Fact(DisplayName = "ParseUrlParameters: Given a path with 2 querystrings and empty parameters Then count of parameters equals 2")]
        public void ParseUrlParameters_GivenAPathWith2QuerystringsAndEmptyParameters_ThenCountOfParametersEquals2()
        {
            string path = "/me/likes?limit=3&offset=2";
            var parameters = new Dictionary<string, object>();

            FacebookAppBase.ParseQueryParametersToDictionary(path, parameters);

            Assert.Equal(2, parameters.Count);
        }

        [Fact(DisplayName = "ParseUrlParameters:  Given a path with 2 querystrings and empty parameters Then parameter values equal to the querystrings")]
        public void ParseUrlParameters_GivenAPathWith2QuerystringsAndEmptyParameters_ThenParameterValuesEqualToTheQuerystrings()
        {
            string path = "/me/likes?limit=3&offset=2";
            var parameters = new Dictionary<string, object>();

            FacebookAppBase.ParseQueryParametersToDictionary(path, parameters);

            Assert.Equal("3", parameters["limit"]);
            Assert.Equal("2", parameters["offset"]);
        }

        [Fact(DisplayName = "ParseUrlParameters: Given a url host is facebook graph with querystring and parameter is empty Then the parameter values are equal to the querystrings")]
        public void ParseUrlParameters_GivenAUrlHostIsFacebookGraphWithQuerystringAndParameterIsEmpty_ThenTheParameterValuesAreEqualToTheQuerystrings()
        {
            string urlWithQueryString = "http://graph.facebook.com/me/likes?limit=3&offset=2";
            var parameters = new Dictionary<string, object>();

            FacebookAppBase.ParseQueryParametersToDictionary(urlWithQueryString, parameters);

            Assert.Equal("3", parameters["limit"]);
            Assert.Equal("2", parameters["offset"]);
        }

        [Fact(DisplayName = "ParseUrlParameters: Given a path with 2 querystrings and empty parameters Then return path equals the path without querystring")]
        public void ParseUrlParameters_GivenAPathWith2QuerystringsAndEmptyParameters_ThenReturnPathEqualsThePathWithoutQuerystring()
        {
            string originalPath = "/me/likes?limit=3&offset=2";
            var parameters = new Dictionary<string, object>();

            var path = FacebookAppBase.ParseQueryParametersToDictionary(originalPath, parameters);

            Assert.Equal(path, "me/likes");
        }

        [Fact(DisplayName = "ParseUrlParameters: Given a path starting with Forward slash and empty parameters Then return path equals the path without forward slash")]
        public void ParseUrlParameters_GivenAPathStartingWithForwardSlashAndEmptyParameters_ThenReturnPathEqualsThePathWithoutForwardSlash()
        {
            string originalPath = "/me/likes";
            string pathWithoutForwardSlash = "me/likes";
            var parameters = new Dictionary<string, object>();

            var path = FacebookAppBase.ParseQueryParametersToDictionary(originalPath, parameters);

            Assert.Equal(pathWithoutForwardSlash, path);
        }

        [Fact(DisplayName = "ParseUrlParameters: Given a path starting with Forward slash with querystring and empty parameters Then return path does not start with forward slash")]
        public void ParseUrlParameters_GivenAPathStartingWithForwardSlashWithQuerystringAndEmptyParameters_ThenReturnPathDoesNotStartWithForwardSlash()
        {
            string originalPathWithQueryString = "/me/likes?limit=3&offset=2";
            var parameters = new Dictionary<string, object>();

            var path = FacebookAppBase.ParseQueryParametersToDictionary(originalPathWithQueryString, parameters);

            Assert.NotEqual('/', path[0]);
        }

        [Fact(DisplayName = "ParseUrlParameters: Given a path starting with Forward slash with querystring and empty parameters Then return path equals the path without forward slash and querystring")]
        public void ParseUrlParameters_GivenAPathStartingWithForwardSlashWithQuerystringAndEmptyParameters_ThenReturnPathEqualsThePathWithoutForwardSlashAndQuerystring()
        {
            string originalPathWithQueryString = "/me/likes?limit=3&offset=2";
            string pathWithoutForwardSlashAndQueryString = "me/likes";
            var parameters = new Dictionary<string, object>();

            var path = FacebookAppBase.ParseQueryParametersToDictionary(originalPathWithQueryString, parameters);

            Assert.Equal(pathWithoutForwardSlashAndQueryString, path);
        }

        [Fact(DisplayName = "FacebookApp: Given a valid access token The session user id should not be null")]
        public void FacebookApp_GivenAValidAccessToken_TheSessionUserIdShouldNotBeNull()
        {
            var fb = new FacebookApp("1249203702|2.h1MTNeLqcLqw__.86400.129394400-605430316|-WE1iH_CV-afTgyhDPc");

            var userId = fb.Session.UserId;

            Assert.NotNull(userId);
        }

        [Fact(DisplayName = "FacebookApp: Given a valid access token The session user id should be correctly extracted from the access token")]
        public void FacebookApp_GivenAValidAccessToken_TheSessionUserIdShouldBeCorrectlyExtractedFromTheAccessToken()
        {
            var fb = new FacebookApp("1249203702|2.h1MTNeLqcLqw__.86400.129394400-605430316|-WE1iH_CV-afTgyhDPc");

            var userId = fb.Session.UserId;

            Assert.Equal("605430316", userId);
        }

        [Fact(DisplayName = "FacebookApp: Given an application access token The session user id should be null")]
        public void FacebookApp_GivenAnApplicationAccessToken_TheSessionUserIdShouldBeNull()
        {
            string appId = "123";
            string appSecret = " A12aB";

            var fb = new FacebookApp(string.Format("{0}|{1}", appId, appSecret));

            var userId = fb.Session.UserId;

            Assert.Null(userId);
        }

        //[Fact(DisplayName = "FacebookApp: Given an invalid access token containing non numeric user id The session user id should be null")]
        //public void FacebookApp_GivenAnInvalidAccessTokenContainingNonNumericUserId_TheSessionUserIdShouldBeNull()
        //{
        //    var fb = new FacebookApp("1249203702|2.h1MTNeLqcLqw__.86400.129394400-6sd05430316|-WE1iH_CV-afTgyhDPc");

        //    var userId = fb.Session.UserId;

        //    Assert.Null(userId);
        //}
    }
}
