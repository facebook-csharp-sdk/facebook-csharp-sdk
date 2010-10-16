using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.Contracts;

namespace Facebook.Web
{
    [TestClass]
    public class TestInitializer
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext tc)
        {
            Contract.ContractFailed += (sender, e) =>
            {
                e.SetUnwind(); // cause code to abort after event
                Assert.Fail(e.FailureKind.ToString() + ":" + e.Message);
            };
        }
    }
}
