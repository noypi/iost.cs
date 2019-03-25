using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using IOSTSdk.Contract.Token;
using IOSTSdk.Contract.System;
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
        readonly string _TestServerUrl = "192.168.254.99:30002";

        readonly string ExamplePrivKey = "3Kq9jHWnoXEXE81BYjqiRYMMds5W8ZPb1M9VbginHNr9TTZcF82Fb5m4vuyC5wocMwjmLomttAcHMfdFREMuUsmi";
        readonly string ExamplePubKey = "42DuuQxdepiQFctVcGHyiBmnEC53otCERRnELXF62aC8";

        readonly string ExamplePrivKey2 = "2hwTHVE6r8AoWPDCZrJP5scsUrCWFKApJJu82VQWreXoW4FiXijuARr1fPeYZSbt9FjvdUeqgiQwBPs4EUitqSY9";
        readonly string ExamplePubKey2 = "2A2tBLFYbMmFqnwih6i6zYMHNoBWBAomhe7ErxcmVGty";

        readonly string ExampleAdminPrivKey = "2yquS3ySrGWPEKywCPzX4RTJugqRh7kJSo5aehsLYPEWkUxBWA39oMrZ7ZxuM4fgyXYs2cPwh5n8aNNpH5x2VyK1";
        readonly string ExampleAdminPubKey = "Gcv8c2tH8qZrUYnKdEEdTtASsxivic2834MQW6mgxqto";
        
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
        public async Task AddNewActiveAndOwnerOfTestAccount()
        {
            var client = new Client(_TestServerUrl);
            var iost = new IOST(client, new Options { ExpirationInMillis = 5000 });

            var tx = iost.NewTransaction()
                         .AuthAssignPermission("saifsolo", "owner", ExamplePubKey2, 200)
                         .AuthAssignPermission("saifsolo", "active", ExamplePubKey2, 200);

            var kc = new Keychain("saifsolo");
            kc.AddKey(
                IOST.Base58Decode(
                    ExamplePrivKey),
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
                IOST.Base58Decode(
                    ExamplePrivKey2),
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
