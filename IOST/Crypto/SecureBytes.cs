namespace IOST.Crypto
{
    using System;
    using System.Security.Cryptography;

    public class SecureBytes
    {
        private readonly byte[] _entropy;
        private readonly byte[] _protected;

        public SecureBytes(byte[] data)
        {
            _entropy = IOST.CryptoRandomSeed(16);
            _protected = ProtectedData.Protect(data, _entropy, DataProtectionScope.CurrentUser);
        }

        public void UseUnprotected(Action<byte[]> action)
        {
            byte[] unprotected = ProtectedData.Unprotect(_protected, _entropy, DataProtectionScope.CurrentUser);
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
