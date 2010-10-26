using Facebook;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Facebook.Tests
{
    
    
    /// <summary>
    ///This is a test class for DateTimeConvertorTest and is intended
    ///to contain all DateTimeConvertorTest Unit Tests
    ///</summary>
    [TestClass]
    public class DateTimeConvertorTest
    {

        [TestMethod]
        public void Convert_To_And_From_Unix_Time_String()
        {
            var s = "1213513200";
            var fbUnix = DateTimeConvertor.FromUnixTime(s);
            var unixTime = DateTimeConvertor.ToUnixTime(fbUnix);
            Assert.AreEqual(s, unixTime.ToString());
        }

        [TestMethod]
        public void Convert_To_And_From_Unix_Time_Double()
        {
            var s = 1213513200;
            var fbUnix = DateTimeConvertor.FromUnixTime(s);
            var unixTime = DateTimeConvertor.ToUnixTime(fbUnix);
            Assert.AreEqual(s, unixTime);
        }

        [TestMethod]
        public void Convert_From_Unix_Time_Double_Correct_Date()
        {
            var unixTime = 1284620400;
            var expected = new DateTime(2010, 9, 16, 0, 0, 0, DateTimeKind.Utc);
            var actual = DateTimeConvertor.FromUnixTime(unixTime);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Convert_From_Unix_Time_String_Correct_Date()
        {
            var unixTime = "1284620400";
            var expected = new DateTime(2010, 9, 16, 0, 0, 0, DateTimeKind.Utc);
            var actual = DateTimeConvertor.FromUnixTime(unixTime);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Convert_Date_To_Unix_Time()
        {
            var dateTime = new DateTime(2010, 9, 16, 0, 0, 0, DateTimeKind.Utc);
            var expected = 1284620400;
            var actual = DateTimeConvertor.ToUnixTime(dateTime);
            Assert.AreEqual(expected, actual);
        }

    }
}
