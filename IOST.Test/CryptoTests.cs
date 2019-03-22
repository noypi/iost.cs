using IOST.Crypto;
using IOST.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Threading.Tasks;

namespace IOST.Test
{
    [TestClass]
    public class CryptoTests
    {
        /// <summary>
        /// https://developers.iost.io/docs/en/6-reference/API.html#request-example
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void TestGetPubKeyEd25519()
        {
            var expectedPubKey = "lDS+SdM+aiVHbDyXapvrsgyKxFg9mJuHWPZb/INBRWY=";
            var expectedPrivKey = "gkpobuI3gbFGstgfdymLBQAGR67ulguDzNmLXEJSWaGUNL5J0z5qJUdsPJdqm+uyDIrEWD2Ym4dY9lv8g0FFZg==";

            var pubk = IOST.CryptoGetPubkeyEd25519(System.Convert.FromBase64String(expectedPrivKey));
            Assert.AreEqual(expectedPubKey, System.Convert.ToBase64String(pubk));
        }

        /// <summary>
        /// https://github.com/iost-official/go-iost/blob/7ebb91a55951f6ddc730c6c8d37ed8efbc541af8/crypto/signature_test.go
        /// </summary>
        [TestMethod]
        public void TestSha3()
        {
            var testData = "c6e193266883a500c6e51a117e012d96ad113d5f21f42b28eb648be92a78f92f";
            var privkey = Cryptography.ECDSA.Hex.HexToBytes(testData);

            var expectedSha3 = "f420b28b56ce97e52adf4778a72b622c3e91115445026cf6e641459ec478dae8";
            var mySha3 = IOST.CryptoHashSha3_256(privkey);
            Assert.AreEqual(expectedSha3, Cryptography.ECDSA.Hex.ToString(mySha3), "sha3 test");

            var expectedPubkeySecp256k1 = "0314bf901a6640033ea07b39c6b3acb675fc0af6a6ab526f378216085a93e5c7a2";
            var myPubkeySecp256k1 = IOST.CryptoGetPubkeySecp256k1Compressed(privkey);
            Assert.AreEqual(expectedPubkeySecp256k1, Cryptography.ECDSA.Hex.ToString(myPubkeySecp256k1), "Secp256k1 get pubkey test");
        }

        /// <summary>
        /// https://github.com/iost-official/go-iost/blob/7ebb91a55951f6ddc730c6c8d37ed8efbc541af8/crypto/signature_test.go
        /// </summary>
        [TestMethod]
        public void TestSecp256k1GetPubkey()
        {
            var testData = "c6e193266883a500c6e51a117e012d96ad113d5f21f42b28eb648be92a78f92f";
            var privkey = Cryptography.ECDSA.Hex.HexToBytes(testData);

            var expectedPubkeySecp256k1 = "0314bf901a6640033ea07b39c6b3acb675fc0af6a6ab526f378216085a93e5c7a2";
            var myPubkeySecp256k1 = IOST.CryptoGetPubkeySecp256k1Compressed(privkey);
            Assert.AreEqual(expectedPubkeySecp256k1, Cryptography.ECDSA.Hex.ToString(myPubkeySecp256k1), "Secp256k1 get pubkey test");
        }
    }
}
