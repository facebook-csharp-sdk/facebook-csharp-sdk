// --------------------------------
// <copyright file="UnixTimeTests.cs" company="Thuzi, LLC">
//     Copyright (c) 2010 Thuzi, LLC (thuzi.com)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Facebook {

    public partial class DateTimeConvertorTest {

       [TestMethod]
       public void Convert_To_Unix_Time() {
           string s = "1213513200";
           var fbUnix = DateTimeConvertor.FromUnixTime(s);
           var unixTime = DateTimeConvertor.ToUnixTime(fbUnix);
           Assert.AreEqual(s, unixTime);
       }
   
   }
}
