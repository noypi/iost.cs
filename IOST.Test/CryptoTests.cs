using IOSTSdk.Crypto;
using IOSTSdk.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace IOSTSdk.Test
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

            var seckey = new SecureBytes(System.Convert.FromBase64String(expectedPrivKey));
            var pubk = IOST.CryptoGetPubkeyEd25519(seckey);
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
            var myPubkeySecp256k1 = IOST.CryptoGetPubkeySecp256k1Compressed(new SecureBytes(privkey));
            Assert.AreEqual(expectedPubkeySecp256k1, Cryptography.ECDSA.Hex.ToString(myPubkeySecp256k1), "Secp256k1 get pubkey test");
        }

        /// <summary>
        /// https://github.com/iost-official/go-iost/blob/7ebb91a55951f6ddc730c6c8d37ed8efbc541af8/crypto/signature_test.go
        /// </summary>
        [TestMethod]
        public void TestSecp256k1GetPubkey()
        {
            var testData = "c6e193266883a500c6e51a117e012d96ad113d5f21f42b28eb648be92a78f92f";
            var seckey = new SecureBytes(Cryptography.ECDSA.Hex.HexToBytes(testData));

            var expectedPubkeySecp256k1 = "0314bf901a6640033ea07b39c6b3acb675fc0af6a6ab526f378216085a93e5c7a2";
            var myPubkeySecp256k1 = IOST.CryptoGetPubkeySecp256k1Compressed(seckey);
            Assert.AreEqual(expectedPubkeySecp256k1, Cryptography.ECDSA.Hex.ToString(myPubkeySecp256k1), "Secp256k1 get pubkey test");
        }

        [TestMethod]
        public void TestXChaCha()
        {
            var message = "some data";
            var password = new SecureBytes(UnicodeEncoding.UTF8.GetBytes("the password"));
            var (cipher, nonce) = SodiumXChaCha20Poly1305.Encrypt(password, UnicodeEncoding.UTF8.GetBytes(message));

            CollectionAssert.AreNotEqual(UnicodeEncoding.UTF8.GetBytes(message), cipher);
            var result = SodiumXChaCha20Poly1305.Decrypt(password, cipher, nonce);
            Assert.AreEqual(message, UnicodeEncoding.UTF8.GetString(result));
        }

        [TestMethod]
        public void TestSecureStream()
        {
            var password = "some password";
            SecureBytes secureBytes = null;

            using (var secureStream = new SecureStream())
            {
                var writer = new StreamWriter(secureStream.Writer);
                writer.Write("some data");
                writer.Flush();
                secureBytes = secureStream.ToSecureBytes();
            }

            string result = string.Empty;
            secureBytes.UseUnprotected(bb => result = UnicodeEncoding.UTF8.GetString(bb));

            Assert.AreEqual(password, result);
        }

#if DEBUG
        [TestMethod]
        public void TestSecurePassword()
        {
            var sec = new SecurePassword();

            sec.Append('a');
            sec.Append('c');
            sec.Append('d');
            Assert.AreEqual("acd", sec.Unsecure(UnicodeEncoding.UTF8));

            sec.InsertAt(1, 'b');
            Assert.AreEqual("abcd", sec.Unsecure(UnicodeEncoding.UTF8));
        }
#endif
    }
}
