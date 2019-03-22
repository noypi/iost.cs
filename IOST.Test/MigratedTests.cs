using IOST.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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

        /// <summary>
        /// See 
        /// - txToBytes() https://github.com/iost-official/go-iost/blob/b201a7af2f8ba2d96476ef65a01c15f0563b415e/sdk/utils.go
        /// - ToBytes()   https://github.com/iost-official/go-iost/blob/master/core/tx/tx.go
        /// </summary>
        /// <returns></returns>
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

            var txBytes = Transaction.ToBytesForSigning(tx, Encoding.ASCII);
            var hash = IOST.CryptoHashSha3_256(txBytes);

            string expectedHash = "/gB8TJQibGI7Kem1v4vJPcJ7vHP48GuShYfd/7NhZ3w=";
            var base64 = System.Convert.ToBase64String(hash);
            Assert.AreEqual(expectedHash, base64, "hashing bytes");

            var expectedPubKey = "lDS+SdM+aiVHbDyXapvrsgyKxFg9mJuHWPZb/INBRWY=";
            var expectedPrivKey = "gkpobuI3gbFGstgfdymLBQAGR67ulguDzNmLXEJSWaGUNL5J0z5qJUdsPJdqm+uyDIrEWD2Ym4dY9lv8g0FFZg==";
            var expectedED25519Sig = "/K1HM0OEbfJ4+D3BmalpLmb03WS7BeCz4nVHBNbDrx3/A31aN2RJNxyEKhv+VSoWctfevDNRnL1kadRVxSt8CA==";

            var seckey = Convert.FromBase64String(expectedPrivKey);
            var actualPubkey = IOST.CryptoGetPubkeyEd25519(seckey);
            Assert.AreEqual(Convert.ToBase64String(actualPubkey), expectedPubKey, "getting public key");

            var signedBytes = IOST.CryptoSignEd25519(hash, seckey);
            var actualSigned = Convert.ToBase64String(signedBytes);
            Assert.AreEqual(expectedED25519Sig, actualSigned, "signing hash");
        }

        /// <summary>
        /// Modified tests from
        /// From https://github.com/iost-official/go-iost/blob/610079ad3d299da2ad1fa8fef389cda39f1236e4/core/tx/tx_test.go
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task TestActionToBytes()
        {
            var tx = new Rpcpb.TransactionRequest();
            tx.Actions.Add(Transaction.NewAction("cont", "abi", "[]"));

            var se = new SimpleEncoder(1000);
            se.TextEncoding = Encoding.ASCII;
            se.PutList<Rpcpb.Action>(tx.Actions, Transaction.ActionToBytes);

            var bytes = se.GetBytes();

            var expectedHexFromBytes = "000000010000001500000004636f6e7400000003616269000000025b5d";
            Assert.AreEqual(expectedHexFromBytes, BitConverter.ToString(bytes).ToLowerInvariant().Replace("-", ""));
        }
    }
}
