// --------------------------------
// <copyright file="GraphReadTests.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Dynamic;

namespace Facebook.Tests.Graph
{

    /*
        * All objects in Facebook can be accessed in the same way:

       •Users: https://graph.facebook.com/btaylor (Bret Taylor)
       •Pages: https://graph.facebook.com/cocacola (Coca-Cola page)
       •Events: https://graph.facebook.com/251906384206 (Facebook Developer Garage Austin)
       •Groups: https://graph.facebook.com/2204501798 (Emacs users group)
       •Applications: https://graph.facebook.com/2439131959 (the Graffiti app)
       •Status messages: https://graph.facebook.com/367501354973 (A status message from Bret)
       •Photos: https://graph.facebook.com/98423808305 (A photo from the Coca-Cola page)
       •Photo albums: https://graph.facebook.com/99394368305 (Coca-Cola's wall photos)
       •Profile pictures: http://graph.facebook.com/jimizim/picture (your profile picture)
       •Videos: https://graph.facebook.com/614004947048 (A Facebook tech talk on Tornado)
       •Notes: https://graph.facebook.com/122788341354 (Note announcing Facebook for iPhone 3.0)

        */

    [TestClass]
    public class GraphReadTests
    {

        private FacebookApp app;
        public GraphReadTests()
        {
            app = new FacebookApp();
            app.MaxRetries = 0;
            app.Session = new FacebookSession
            {
                AccessToken = ConfigurationManager.AppSettings["AccessToken"],
            };
        }

        [TestMethod]
        public void Read_Likes()
        {
            dynamic likesResult = app.Api("/totten/likes");
            dynamic likesData = likesResult.data;
            Assert.IsNotNull(likesData);

            dynamic total = likesData.Count;
            Assert.AreNotEqual(0, total);

            var firstLikePageName = likesData[0].name;
            Assert.AreNotEqual(String.Empty, firstLikePageName);
        }

        [TestMethod]
        public void Read_Public_Fan_Page_Id()
        {
            dynamic pageResult = app.Api("/outback");
            Assert.AreEqual(pageResult.id, "48543634386");
        }

        [TestMethod]
        public void Read_User_Info()
        {
            dynamic result = app.Api("/me");
            Assert.AreEqual(result.name, "Nathan Tester");
        }

        [TestMethod]
        public void Read_Application_Info()
        {
            dynamic result = app.Api("/2439131959");
            Assert.AreEqual(result.category, "Just For Fun");
        }

        [TestMethod]
        public void Read_Photo_Info()
        {
            dynamic result = app.Api("/98423808305");
            Assert.AreEqual(result.from.name, "Coca-Cola");
        }

        [TestMethod]
        public void Read_Event()
        {
            dynamic result = app.Api("/331218348435");
            Assert.AreEqual(result.venue.city, "Austin");
        }

        [TestMethod]
        public void ReadPublicProfile()
        {
            dynamic result = app.Api("/totten");
            Assert.AreEqual("Nathan", result.first_name);
        }

        [TestMethod]
        [ExpectedException(typeof(FacebookOAuthException))]
        public void get_user_likes_should_throw_oauth()
        {
            dynamic parameters = new ExpandoObject();
            parameters.access_token = "invalidtoken";
            dynamic result = app.Api("/totten/likes", parameters);
            Assert.Fail();
        }
    }
}
