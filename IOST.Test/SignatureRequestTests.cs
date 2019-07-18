using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Google.Protobuf;
using ChainPay.Models;
using ChainPay;
using System.Threading.Tasks;
using Rpcpb;

namespace IOSTSdk.Test
{
    [TestClass]
    public class SignatureRequestTests
    {
        [TestInitialize]
        public void Initialize()
        {

        }

        [TestMethod]
        public async Task TestClient()
        {
            /*var client = Client.NewTestNet();
            var task = client.GetAccountAsync(new Rpcpb.GetAccountRequest()
            {
                ByLongestChain = true,
                Name = "saifsolo"
            }).ResponseAsync;

            var result = await task;*/

            var typet = typeof(Google.Api.AnnotationsReflection);
            var desc1 = Google.Api.AnnotationsReflection.Descriptor;
            
            //var desc = (result as IMessage).Descriptor;
        }

        [TestMethod]
        public void TestReadGenerateSR()
        {
            var iost = new IOST(null);

            var tx = iost.NewTransaction();
            tx.Transfer("somelongtoken123", "abcdef123456789", "abcdef123456789", 12345678.12345678, "some very long memo 123456");

            var secbytes = IOST.CryptoGeneratePrivateKeyEd25519(IOST.CryptoRandomSeed(32));
            var kc = new Keychain("somelongtoken123");
            kc.AddKey(secbytes, "active");

            kc.Sign(tx);
            
            var sigreq = new SignatureRequest()
            {
                BlockchainCode = "IOST",
                BlockchainName = "Internet Of Services",
                HashAlgo = "SHA3-256",
                MessageHash = "some hash",
                TransferDetails = tx.TxRequest.ToByteString()
            };

            var ss = Common.CreateSignature(sigreq, "Request");

            var pubk = "Gcv8c2tH8qZrUYnKdEEdTtASsxivic2834MQW6mgxqto";
            tx.CreateAccount("newname", "creator", pubk, pubk);

            var bb1 = tx.BytesForSigning();

            
        }
    }
}
