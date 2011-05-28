namespace Facebook.Tests.FacebookUtils.ParseQueryParametersToDictionary
{
    using System.Dynamic;
    using System.IO;
    using Facebook;
    using Xunit;

    public class BuildMediaObjectPostData
    {
        [Fact(Skip = "fix test")]
        public void Test()
        {
            /*
            // TODO: give proper name for this test.
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
            byte[] actual = FacebookClient.BuildMediaObjectPostData(parameters, boundary);

            Assert.Equal(127231, actual.Length);
             * */
        }
    }
}