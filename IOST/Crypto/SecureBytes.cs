namespace IOST.Crypto
{
    using System;
    using System.Security.Cryptography;

    public class SecureBytes
    {
        readonly byte[] _entropy;
        private readonly byte[] _protected;

        public SecureBytes(byte[] data)
        {
            RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();
            _entropy = new byte[16];
            rand.GetBytes(_entropy);

            _protected = data;
            ProtectedData.Protect(_protected, _entropy, DataProtectionScope.CurrentUser);
        }

        public void UseUnprotected(Action<byte[]> action)
        {
            byte[] unprotected = new byte[_protected.Length];
            Array.Copy(_protected, unprotected, _protected.Length);

            ProtectedData.Unprotect(unprotected, _entropy, DataProtectionScope.CurrentUser);
            action(unprotected);

            DestroyData(unprotected);
        }

        private static void DestroyData(byte[] data)
        {
            for(int i=0; i<data.Length; i++)
            {
                data[i] = 0;
            }
        }
    }
}
