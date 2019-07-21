namespace IOSTSdk.Crypto
{
    using IOSTSdk.Helpers;
    using System;
    using System.Security.Cryptography;

    public class SecureBytesWin : ISecureBytes
    {
        private readonly byte[] _entropy;
        private readonly byte[] _protected;

        public SecureBytesWin(byte[] data)
        {
            _entropy = IOST.CryptoRandomSeed(16);
            _protected = ProtectedData.Protect(data, _entropy, DataProtectionScope.CurrentUser);
        }

        public void UseUnprotected(Action<byte[]> action)
        {
            byte[] unprotected = ProtectedData.Unprotect(_protected, _entropy, DataProtectionScope.CurrentUser);
            action(unprotected);
            DataHelper.DestroyData(unprotected);
        }
    }
}
