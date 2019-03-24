using IOSTSdk.Crypto;
using IOSTSdk.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace IOSTSdk.Test
{
    [TestClass]
    public class HelperTests
    {
        //private string _TestServerUrl = "localhost:30002";
        private readonly string _TestServerUrl = "192.168.254.99:30002";

        [TestMethod]
        public async Task TestGetNodeInfo()
        {
            var client = new Client(_TestServerUrl);
            var nodeInfoResponse = await client.GetNodeInfo();
            Assert.IsNotNull(nodeInfoResponse);
        }

        [TestMethod]
        public void TestDefaultOptions()
        {
            var options = new Options { };
            Assert.AreEqual(1, options.GasRatio);
            Assert.AreEqual(1000000, options.GasLimit);
            Assert.AreEqual(0, options.Delay);
            Assert.AreEqual((long)TimeSpan.FromSeconds(90).TotalMilliseconds, options.ExpirationInMillis);
        }

        [TestMethod]
        public void TestMillisToNano()
        {
            var ns = DateHelper.TicksToNanoseconds(TimeSpan.FromSeconds(1).Ticks);
            const long million = 1000000;
            const long thousand = 1000;
            Assert.AreEqual(thousand * million, ns);
        }

        [TestMethod]
        public void TestSecureBytes()
        {
            byte[] data = new byte[] { 1, 2, 3, 4, 5 };
            var secure = new SecureBytes(data);

            secure.UseUnprotected((bb) =>
            {
                CollectionAssert.AreEqual(data, bb);
            });
        }
    }
}
