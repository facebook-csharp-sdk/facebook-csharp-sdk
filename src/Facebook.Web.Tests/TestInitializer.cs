using System.Diagnostics.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
