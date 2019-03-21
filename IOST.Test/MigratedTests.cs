using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading.Tasks;

namespace IOST.Test
{
    /// <summary>
    /// Migrates tests from https://github.com/iost-official/go-sdk/blob/master/sdk_test.go
    /// </summary>
    [TestClass]
    public class MigratedTests
    {
        //private string _TestServerUrl = "localhost:30002";
        private readonly string _TestServerUrl = "192.168.254.99:30002";

        [TestMethod]
        public async Task TestGet()
        {
            var client = new Client(_TestServerUrl);

            var nodeInfoResponse = await client.GetNodeInfo();
            Debug.WriteLine(nodeInfoResponse);
            Assert.IsNotNull(nodeInfoResponse);

            var chainInfoResponse = await client.GetChainInfo();
            Debug.WriteLine(chainInfoResponse);
            Assert.IsNotNull(chainInfoResponse);
        }

        [TestMethod]
        public async Task TestSendTx()
        {
            var client = new Client(_TestServerUrl);
            var iost = new IOST(client, new Options { ExpirationInMillis = 5000 });

            var tx = iost.CreateTx()
                        .Transfer("iost", "admin", "admin", 10.000, "");

            var kc = new Keychain("admin");
            kc.AddKey(IOST.Base58Decode("2yquS3ySrGWPEKywCPzX4RTJugqRh7kJSo5aehsLYPEWkUxBWA39oMrZ7ZxuM4fgyXYs2cPwh5n8aNNpH5x2VyK1"), "active");

            var hash = await iost.Send(tx, kc, "active");
            Debug.WriteLine(hash);
            Assert.IsFalse(string.IsNullOrEmpty(hash));
        }
    }
}
