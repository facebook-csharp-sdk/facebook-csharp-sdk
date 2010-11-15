using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using Facebook.Graph;

namespace Facebook.Tests
{
    [TestClass]
    public class TypeObjectTests
    {
        private FacebookApp app;
        public TypeObjectTests()
        {
            app = new FacebookApp();
            app.MaxRetries = 0;
            app.Session = new FacebookSession
            {
                AccessToken = ConfigurationManager.AppSettings["AccessToken"],
            };
        }

        [TestMethod]
        public void Get_User_Info_Typed()
        {
            var user = app.Api<User>("/totten");
            Assert.IsNotNull(user.FirstName);

        }
    }
}
