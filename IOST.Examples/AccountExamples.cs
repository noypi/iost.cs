using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using IOSTSdk.Contract.Token;
using IOSTSdk.Contract.System;
using IOSTSdk.Contract.Economic;
using IOSTSdk.Crypto;
using IOSTSdk.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IOSTSdk.Examples
{
    /// <summary>
    /// Migrates tests from https://github.com/iost-official/go-sdk/blob/master/sdk_test.go
    /// </summary>
    [TestClass]
    public class AccountExamples
    {
        //private string _TestServerUrl = "localhost:30002";
        static readonly string _TestServerUrl = "192.168.254.99:30002";

        static readonly string ExamplePrivKey = "3Kq9jHWnoXEXE81BYjqiRYMMds5W8ZPb1M9VbginHNr9TTZcF82Fb5m4vuyC5wocMwjmLomttAcHMfdFREMuUsmi";
        readonly string ExamplePubKey = "42DuuQxdepiQFctVcGHyiBmnEC53otCERRnELXF62aC8";

        static readonly string ExamplePrivKey2 = "2hwTHVE6r8AoWPDCZrJP5scsUrCWFKApJJu82VQWreXoW4FiXijuARr1fPeYZSbt9FjvdUeqgiQwBPs4EUitqSY9";
        static readonly string ExamplePubKey2 = "2A2tBLFYbMmFqnwih6i6zYMHNoBWBAomhe7ErxcmVGty";

        static readonly string ExampleAdminPrivKey = "2yquS3ySrGWPEKywCPzX4RTJugqRh7kJSo5aehsLYPEWkUxBWA39oMrZ7ZxuM4fgyXYs2cPwh5n8aNNpH5x2VyK1";
        static readonly string ExampleAdminPubKey = "Gcv8c2tH8qZrUYnKdEEdTtASsxivic2834MQW6mgxqto";

        static readonly string TestNetSaifsolo2PrivK = "5CtYqbpK4SfYjczNTsxqJbk9kcFkiPHRXuNtTSpuUT95tRu3RBALRroUXYShkVxQ6ZEioqVwXkn4aL7Ci9SGWQEw";
        static readonly string TestNetSaifsolo2PubK = IOST.Base58Encode(
                                                 IOST.CryptoGetPubkeyEd25519(new SecureBytes(IOST.Base58Decode(TestNetSaifsolo2PrivK))));

        [TestMethod]
        public async Task CreateNewAccount()
        {
            var client = Client.NewTestNet();
            var iost = new IOST(client, new Options { ExpirationInMillis = 5000, ChainId = 1023 });

            var tx = iost.NewTransaction()
                         .CreateAccount("saifsolo4", "saifsolo3", TestNetSaifsolo2PubK, TestNetSaifsolo2PubK);
            
            var kc = new Keychain("saifsolo2");
            kc.AddKey(
                new SecureBytes(IOST.Base58Decode(
                    TestNetSaifsolo2PrivK)),
                    "active");
            tx.AddApprove("*", "unlimited");
            kc.Sign(tx);
            var hash = await iost.Send(tx);
        }

#if false
        [TestMethod]
        public async Task CreateNewAccount2()
        {
            string k = "";
            string expected_pk = "";
            string pk = IOST.Base58Encode(
                                IOST.CryptoGetPubkeyEd25519(new SecureBytes(IOST.Base58Decode(k))));
            Assert.AreEqual(expected_pk, pk);

            var newk = "";
            var newpk = IOST.Base58Encode(
                                IOST.CryptoGetPubkeyEd25519(new SecureBytes(IOST.Base58Decode(newk)))); ;
            var expected_newpk = "";
            Assert.AreEqual(expected_newpk, newpk);

            var client = Client.NewTestNet();
            var iost = new IOST(client, new Options { ExpirationInMillis = 5000, ChainId = 1023 });

            var tx = iost.NewTransaction()
                         .CreateAccount("saifsolo3", "saifsolo2", newpk, newpk); 

            var kc = new Keychain("saifsolo2");
            kc.AddKey(
                new SecureBytes(IOST.Base58Decode(
                    k)),
                    "active");
            tx.AddApprove("*", "unlimited");
            kc.Sign(tx);
            var hash = await iost.Send(tx);
        }
#endif
        [TestMethod]
        public async Task TransferIOST()
        {
            var client = Client.NewTestNet();
            var iost = new IOST(client, new Options { ExpirationInMillis = 5000, ChainId = 1023 });

            var tx = iost.NewTransaction()
                         .Transfer("iost", "saifsolo2", "saifsolo3", 1000, "");

            var kc = new Keychain("saifsolo2");
            kc.AddKey(
                new SecureBytes(IOST.Base58Decode(
                    TestNetSaifsolo2PrivK)),
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
                new SecureBytes(IOST.Base58Decode(
                    ExampleAdminPrivKey)),
                    "active");

            kc.Sign(tx);
            var hash = await iost.Send(tx);
        }

        [TestMethod]
        public async Task TransferToAccount()
        {
            var client = Client.NewTestNet();
            var iost = new IOST(client, new Options { ExpirationInMillis = 5000 });

            var tx = iost.NewTransaction()
                         .Transfer("iost", "admin", "saifsolo", 1000, "");

            var kc = new Keychain("noypi");
            kc.AddKey(
                new SecureBytes(IOST.Base58Decode(
                    ExampleAdminPrivKey)),
                    "active");
            var hash = await iost.Send(tx);

        }

        [TestMethod]
        public async Task AddNewActiveAndOwnerOfTestAccount()
        {
            var client = new Client(_TestServerUrl);
            var iost = new IOST(client, new Options { ExpirationInMillis = 5000 });

            var tx = iost.NewTransaction()
                         .AuthAssignPermission("saifsolo", "owner", ExamplePubKey2, 200)
                         .AuthAssignPermission("saifsolo", "active", ExamplePubKey2, 200);

            var kc = new Keychain("saifsolo");
            kc.AddKey(
                new SecureBytes(IOST.Base58Decode(
                    ExamplePrivKey)),
                    "active");

            kc.Sign(tx);
            var hash = await iost.Send(tx);
        }

        [TestMethod]
        public async Task RevokeOldKeysOfTestAccount()
        {
            var client = new Client(_TestServerUrl);
            var iost = new IOST(client, new Options { ExpirationInMillis = 5000 });

            var tx = iost.NewTransaction()
                         .AuthRevokePermission("saifsolo", "owner", ExamplePubKey)
                         .AuthRevokePermission("saifsolo", "active", ExamplePubKey)
                         .AuthAssignPermission("saifsolo", "owner", ExamplePubKey2, 100)
                         .AuthAssignPermission("saifsolo", "active", ExamplePubKey2, 100); ;

            var kc = new Keychain("saifsolo");
            kc.AddKey(
                new SecureBytes(IOST.Base58Decode(
                    ExamplePrivKey2)),
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
                new SecureBytes(IOST.Base58Decode(
                    ExampleAdminPrivKey)),
                    "active");

            kc.Sign(tx);
            var hash = await iost.Send(tx);
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
    }
}
