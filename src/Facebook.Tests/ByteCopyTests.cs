using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Facebook.Tests
{
    [TestClass]
    public class ByteCopyTests
    {
        [TestMethod]
        public void Test_Byte_Copy()
        {
            List<byte[]> byteList = new List<byte[]>();

            for (int i = 0; i < 5; i++)
            {
                var s = string.Format("This is test string number {0}.", i);
                var b = Encoding.UTF8.GetBytes(s);
                byteList.Add(b);
            }

            int length = 0;
            byteList.ForEach((ary) => { length += ary.Length; });
            var resultBytes = new byte[length];
            int offset = 0;
            byteList.ForEach((ary) =>
            {
                offset += ary.Length;
                Buffer.BlockCopy(ary, 0, resultBytes, offset - ary.Length, ary.Length);
            });

            var resultString = Encoding.UTF8.GetString(resultBytes);

            Assert.AreEqual("This is test string number 0.This is test string number 1.This is test string number 2.This is test string number 3.This is test string number 4.", resultString);
        }
    }
}
