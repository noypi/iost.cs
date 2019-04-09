using IOSTSdk.Crypto;
using IOSTSdk.Keystore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IOSTSdk.Test
{
    [TestClass]
    public class KeystoreFileTests
    {
        static readonly string TestNetSaifsolo2PrivK = "5CtYqbpK4SfYjczNTsxqJbk9kcFkiPHRXuNtTSpuUT95tRu3RBALRroUXYShkVxQ6ZEioqVwXkn4aL7Ci9SGWQEw";


        [TestInitialize]
        public void Initialize()
        {

        }

        [TestMethod]
        public void TestLoadSaveFile()
        {
            var tmpfile = Path.GetTempFileName();
            var ks = KeystoreRegistry.Create(KeystoreType.FromFile, $"file={tmpfile}");

            var password = createPassword("some password").ToSecureBytes();
            var privkey = new SecureBytes(IOST.Base58Decode(TestNetSaifsolo2PrivK));
            ks.AddKey(privkey, password, "some new password");

            ks.Store();

            var ks2 = KeystoreRegistry.Create(KeystoreType.FromFile, $"file={tmpfile}");
            EncryptedKeysAreEqual(ks.EncryptedKeys, ks2.EncryptedKeys);

            var secPrivKey = ks2.EncryptedKeys[0].Decrypt(createPassword("some password").ToSecureBytes());
            secPrivKey.UseUnprotected(pk => CollectionAssert.AreEqual(
                                                        IOST.Base58Decode(TestNetSaifsolo2PrivK), 
                                                        pk));

            File.Delete(tmpfile + ".backup");
        }

        [TestMethod]
        public void TestChangePassword()
        {
            var tmpfile = Path.GetTempFileName();
            var ks = KeystoreRegistry.Create(KeystoreType.FromFile, $"file={tmpfile}");

            var password = createPassword("some password").ToSecureBytes();
            var privkey = new SecureBytes(IOST.Base58Decode(TestNetSaifsolo2PrivK));
            ks.AddKey(privkey, password, "some new password");

            ks.ChangePassword(createPassword("some password").ToSecureBytes(), createPassword("new password").ToSecureBytes());

            var secPrivKey = ks.EncryptedKeys[0].Decrypt(createPassword("new password").ToSecureBytes());
            secPrivKey.UseUnprotected(pk => CollectionAssert.AreEqual(
                                                        IOST.Base58Decode(TestNetSaifsolo2PrivK),
                                                        pk));

            File.Delete(tmpfile + ".backup");
        }

        static SecurePassword createPassword(string password)
        {
            var secpass = new SecurePassword();
            foreach(var c in password)
            {
                secpass.Append(c);
            }
            return secpass;
        }

        static void EncryptedKeysAreEqual(EncryptedKey[] k1, EncryptedKey[] k2)
        {
            Assert.AreEqual(k1.Length, k2.Length);
            for(int i = 0; i < k1.Length; i++)
            {
                Assert.AreEqual(k1[i].Algo, k2[i].Algo);
                Assert.AreEqual(k1[i].Cipher, k2[i].Cipher);
                Assert.AreEqual(k1[i].Label, k2[i].Label);
                Assert.AreEqual(k1[i].Nonce, k2[i].Nonce);
            }
        }
    }
}
