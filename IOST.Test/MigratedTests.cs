using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using IOSTSdk.Contract.Token;
using IOSTSdk.Crypto;
using IOSTSdk.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IOSTSdk.Test
{
    /// <summary>
    /// Migrates tests from https://github.com/iost-official/go-sdk/blob/master/sdk_test.go
    /// </summary>
    [TestClass]
    public class MigratedTests
    {
        //private string _TestServerUrl = "localhost:30002";
        private readonly string _TestServerUrl = "192.168.254.99:30002";

        private readonly string ExamplePrivKey = "3Kq9jHWnoXEXE81BYjqiRYMMds5W8ZPb1M9VbginHNr9TTZcF82Fb5m4vuyC5wocMwjmLomttAcHMfdFREMuUsmi";
        private readonly string ExamplePubKey = "42DuuQxdepiQFctVcGHyiBmnEC53otCERRnELXF62aC8";

        private readonly string ExampleAdminPrivKey = "2yquS3ySrGWPEKywCPzX4RTJugqRh7kJSo5aehsLYPEWkUxBWA39oMrZ7ZxuM4fgyXYs2cPwh5n8aNNpH5x2VyK1";
        private readonly string ExampleAdminPubKey = "Gcv8c2tH8qZrUYnKdEEdTtASsxivic2834MQW6mgxqto";

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

            var kc = new Keychain("admin");
            kc.AddKey(
                IOST.Base58Decode(
                    ExamplePrivKey), 
                    "active");

            var tx = iost.NewTransaction()
                         .Transfer("iost", "admin", "admin", 10.000, "");
            kc.Sign(tx);
            var hash = await iost.Send(tx);
            Debug.WriteLine(hash);
            Assert.IsFalse(string.IsNullOrEmpty(hash));
        }

        [TestMethod]
        public async Task CreateNewAccount()
        {
            var client = new Client(_TestServerUrl);
            var iost = new IOST(client, new Options { ExpirationInMillis = 5000 });

            var tx = iost.NewTransaction()
                         .NewAccount("saifsolo", "admin", ExamplePubKey, ExamplePubKey, 1024, 100000);

            tx.AddApprove("*", "unlimited");

            var kc = new Keychain("admin");
            kc.AddKey(
                IOST.Base58Decode(
                    ExampleAdminPrivKey),
                    "active");

            kc.Sign(tx);
            var hash = await iost.Send(tx);
        }

        [TestMethod]
        public async Task CreateDepositToTestAccount()
        {
            var client = new Client(_TestServerUrl);
            var iost = new IOST(client, new Options { ExpirationInMillis = 5000 });

            var tx = iost.NewTransaction()
                         .Transfer("iost", "admin", "saifsolo", 10000, "");

            var kc = new Keychain("admin");
            kc.AddKey(
                IOST.Base58Decode(
                    ExampleAdminPrivKey),
                    "active");

            kc.Sign(tx);
            var hash = await iost.Send(tx);
        }

        [TestMethod]
        public async Task CreateDepositToTestAccountPubkey()
        {
            var client = new Client(_TestServerUrl);
            var iost = new IOST(client, new Options { ExpirationInMillis = 5000 });

            var tx = iost.NewTransaction()
                         .Transfer("iost", "admin", ExamplePubKey, 500, "");

            var kc = new Keychain("admin");
            kc.AddKey(
                IOST.Base58Decode(
                    ExampleAdminPrivKey),
                    "active");

            kc.Sign(tx);
            var hash = await iost.Send(tx);
        }

        [TestMethod]
        public async Task TestBalance()
        {
            var client = new Client(_TestServerUrl);
            var iost = new IOST(client, new Options { ExpirationInMillis = 5000 });

            var kc = new Keychain("admin");
            kc.AddKey(
                IOST.Base58Decode(
                    ExampleAdminPrivKey),
                    "active");

            var adminPubkey = IOST.Base58Encode(kc.GetPublicKey("active"));
            var tx = iost.NewTransaction()
                         .TokenBalanceOf("token.iost", adminPubkey);

            kc.Sign(tx);
            var hash = await iost.Send(tx);
            Debug.WriteLine(hash);
            Assert.IsFalse(string.IsNullOrEmpty(hash));
        }

        /// <summary>
        /// Example generating keys
        /// </summary>
        [TestMethod]
        public void PrintNewPrivateKeyForTesting()
        {
            var seckey = IOST.CryptoGeneratePrivateKeyEd25519(IOST.CryptoRandomSeed(32));
            var pubkey = IOST.CryptoGetPubkeyEd25519(seckey);

            var base58 = string.Empty;
            seckey.UseUnprotected(privkey => base58 = IOST.Base58Encode(privkey));
            Debug.WriteLine("base58 private key:" + base58);
            Debug.WriteLine("base58 public key:" + IOST.Base58Encode(pubkey));
        }

        /// <summary>
        /// Example generating keys
        /// </summary>
        [TestMethod]
        public void PrintPublickKeyFroTesting()
        {
            var seckey = new SecureBytes(IOST.Base58Decode(ExamplePrivKey));
            var pubkey = IOST.CryptoGetPubkeyEd25519(seckey);

            var base58 = IOST.Base58Encode(pubkey);
            Debug.WriteLine("base58 private key:" + ExamplePrivKey);
            Debug.WriteLine("base58 public key:" + base58);
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
            var base64 = Convert.ToBase64String(hash);
            Assert.AreEqual(expectedHash, base64, "hashing bytes");

            var expectedPubKey = "lDS+SdM+aiVHbDyXapvrsgyKxFg9mJuHWPZb/INBRWY=";
            var expectedPrivKey = "gkpobuI3gbFGstgfdymLBQAGR67ulguDzNmLXEJSWaGUNL5J0z5qJUdsPJdqm+uyDIrEWD2Ym4dY9lv8g0FFZg==";
            var expectedED25519Sig = "/K1HM0OEbfJ4+D3BmalpLmb03WS7BeCz4nVHBNbDrx3/A31aN2RJNxyEKhv+VSoWctfevDNRnL1kadRVxSt8CA==";

            var seckey = new SecureBytes(Convert.FromBase64String(expectedPrivKey));
            var actualPubkey = IOST.CryptoGetPubkeyEd25519(seckey);
            Assert.AreEqual(Convert.ToBase64String(actualPubkey), expectedPubKey, "getting public key");

            var signedBytes = IOST.CryptoSignEd25519(hash, seckey);
            var actualSigned = Convert.ToBase64String(signedBytes);
            Assert.AreEqual(expectedED25519Sig, actualSigned, "signing hash");

            foreach(var sign in tx.PublisherSigs)
            {
                Assert.IsTrue(
                        IOST.CryptoVerifyEd25519(sign.Signature_.ToByteArray(), txBytes, actualPubkey), 
                        $"verifying signature: {sign.Signature_.ToBase64()}");
            }
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
            se.PutList<Rpcpb.Action>(tx.Actions, Transaction.ActionToBytes);

            var bytes = se.GetBytes();

            var expectedHexFromBytes = "000000010000001500000004636f6e7400000003616269000000025b5d";
            Assert.AreEqual(expectedHexFromBytes, BitConverter.ToString(bytes).ToLowerInvariant().Replace("-", ""));
        }
    }
}
