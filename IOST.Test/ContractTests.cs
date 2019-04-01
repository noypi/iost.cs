using IOSTSdk;
using IOSTSdk.Crypto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace IOSTSdk.Test
{
    [TestClass]
    public class ContractTests
    {
        private readonly string _TestServerUrl = "192.168.254.99:30002";

        static readonly string ExampleAdminPrivKey = "2yquS3ySrGWPEKywCPzX4RTJugqRh7kJSo5aehsLYPEWkUxBWA39oMrZ7ZxuM4fgyXYs2cPwh5n8aNNpH5x2VyK1";
        static readonly string ExampleAdminPubKey = "Gcv8c2tH8qZrUYnKdEEdTtASsxivic2834MQW6mgxqto";

        static readonly string TestNetSaifsolo2PrivK = "5CtYqbpK4SfYjczNTsxqJbk9kcFkiPHRXuNtTSpuUT95tRu3RBALRroUXYShkVxQ6ZEioqVwXkn4aL7Ci9SGWQEw";
        static readonly string TestNetSaifsolo2PubK = IOST.Base58Encode(
                                                 IOST.CryptoGetPubkeyEd25519(new SecureBytes(IOST.Base58Decode(TestNetSaifsolo2PrivK))));

        static string HelloWorldJS =
                        @"class HelloWorld {" +
                        @"    init() {} // needs to provide an init function that will be called during deployment" +
                        @"    hello(someone) {" +
                        @"        return 'hello, ' + someone;" +
                        @"    }" +
                        @"}"+
                        @"module.exports = HelloWorld;";

        static string HelloWorldABI =
                        @"{" +
                        @"  'lang': 'javascript'," +
                        @"  'version': '1.0.0'," +
                        @"  'abi': [" +
                        @"          {" +
                        @"              'name' : 'hello'," +
                        @"              'args' : [ 'string ' ]" +
                        @"          }" +
                        @"         ]" +
                        @"}";

        Client _client = null;
        IOST _iost = null;

        [TestInitialize]
        public async Task TestInitalize()
        {
            _client = new Client(_TestServerUrl);
            _iost = new IOST(_client, new Options { ExpirationInMillis = 5000 });

            var tx = _iost.NewTransaction()
                         .CreateAccount("saifsolo2", "admin", TestNetSaifsolo2PubK, TestNetSaifsolo2PubK);

            var kc = new Keychain("admin");
            kc.AddKey(
                new SecureBytes(IOST.Base58Decode(
                    ExampleAdminPrivKey)),
                    "active");
            tx.AddApprove("*", "unlimited");
            kc.Sign(tx);
            var hash = await _iost.Send(tx);
        }

        [TestMethod]
        public async Task TestPublishContract()
        {
            var client = new Client(_TestServerUrl);
            var iost = new IOST(client);
            var tx = iost.NewTransaction();

            using (var memJs = new MemoryStream(UnicodeEncoding.UTF8.GetBytes(HelloWorldJS)))
            using (var readerJs = new StreamReader(memJs))
            using (var memAbi = new MemoryStream(UnicodeEncoding.UTF8.GetBytes(HelloWorldABI)))
            using (var readerAbi = new StreamReader(memAbi))
            {
                tx.PublishContract(readerAbi, readerJs);
            }

            var kc = new Keychain("saifsolo2");
            kc.AddKey(
                new SecureBytes(IOST.Base58Decode(
                    TestNetSaifsolo2PrivK)),
                    "active");
            tx.AddApprove("*", "unlimited");
            kc.Sign(tx);
            var hash = await _iost.Send(tx);

            Debug.WriteLine("hash: ", hash);
        }
    }
}
