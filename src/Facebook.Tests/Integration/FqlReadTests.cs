using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Facebook.Linq;
using System.Dynamic;

namespace Facebook.Tests.Fql
{
    [TestClass]
    public class FqlReadTests
    {
        //private FacebookApp app;
        //public FqlReadTests()
        //{
        //    app = new FacebookApp(TestHelper.FacebookSettings);
        //    app.Session = new FacebookSession
        //    {
        //        AccessToken = TestHelper.AuthToken,
        //    };
        //}
        //[TestMethod]
        //public void Get_Albums()
        //{
        //    var db = new FqlDB();

        //    var query = from a in db.album
        //                where a.Owner == 537883665
        //                select new { a.Aid, a.Description, a.Name };
        //    var q = query.ToString();

        //    dynamic parameters = new ExpandoObject();
        //    parameters.query = q;
        //    parameters.method = "fql.query";
        //    dynamic result = app.Api(parameters);

        //    //var q2 = from a in db.album
        //    //         where a.Owner (from ab in db.album where ab.Owner == 537883665);

        //    // List<int> IdsToFind = new List<int>() {2, 3, 4};

        //    //db.Users
        //    //.Where(u => SqlMethods.Like(u.LastName, "%fra%"))
        //    //.Where(u =>
        //    //    db.CompanyRolesToUsers
        //    //    .Where(crtu => IdsToFind.Contains(crtu.CompanyRoleId))
        //    //    .Select(crtu =>  crtu.UserId)
        //    //    .Contains(u.Id)
        //    //)


        //    //var q2 = db.album
        //    //    .Where(a =>
        //    //        db.album
        //    //        .Where(ab => ab.Owner == 537883665))
        //    //        .Select(ab => ab.Name));


        //}
    }
}
