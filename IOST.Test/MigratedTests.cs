using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Text;
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

        /// <summary>
        /// From https://developers.iost.io/docs/en/6-reference/API.html#request-example
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestTxSign()
        {
            var tx = new Rpcpb.TransactionRequest()
            {
                Time = 1544709662543340000,
                Expiration = 1544709692318715000,
                GasRatio = 1,
                GasLimit = 500000,
                Delay = 0,
                ChainId = 1024,
                Publisher = "testaccount"
            };
            tx.Actions.Add(Transaction.NewAction("token.iost", "transfer", "[\"iost\", \"testaccount\", \"anothertest\", \"100\", \"this is an example transfer\"]"));
            tx.AmountLimit.Add(Transaction.NewAmountLimit("*", "unlimited"));

            var txBytes = Transaction.ToBytesForSigning(tx);
            var hash = IOST.CryptoHashSha3_256(txBytes);

            string expectedHash = "/gB8TJQibGI7Kem1v4vJPcJ7vHP48GuShYfd/7NhZ3w=";
            Assert.AreEqual(expectedHash, System.Convert.ToBase64String(hash));

            var expectedPubKey = "lDS+SdM+aiVHbDyXapvrsgyKxFg9mJuHWPZb/INBRWY=";
            var expectedPrivKey = "gkpobuI3gbFGstgfdymLBQAGR67ulguDzNmLXEJSWaGUNL5J0z5qJUdsPJdqm+uyDIrEWD2Ym4dY9lv8g0FFZg==";
            var expectedED25519Sig = "/K1HM0OEbfJ4+D3BmalpLmb03WS7BeCz4nVHBNbDrx3/A31aN2RJNxyEKhv+VSoWctfevDNRnL1kadRVxSt8CA==";

           }
    }
}
