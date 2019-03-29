namespace IOSTSdk.Crypto
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using IOSTSdk.Helpers;
    
    public class SecurePassword
    {
        private byte[] _entropy;
        private byte[] _protected = null;

        public SecurePassword() => Reset();

        public void Reset()
        {
            _protected = null;
            _entropy = IOST.CryptoRandomSeed(16);
        }

        public void InsertAt(int i, char c)
        {
            byte[] unprotected = (_protected != null) ?
                                        Unprotect() :
                                        unprotected = new byte[] {};

            byte[] newdata = new byte[unprotected.Length + 1];
            if (i < 0)
            {
                // do append
                i = unprotected.Length;
            }
            Array.Copy(unprotected, 0, newdata, 0, i);
            newdata[i] = (byte)c;
            Array.Copy(unprotected, i, newdata, i+1, unprotected.Length - i);

            _protected = ProtectedData.Protect(newdata, _entropy, DataProtectionScope.CurrentUser);
            DataHelper.DestroyData(unprotected);
            DataHelper.DestroyData(newdata);
        }
#if DEBUG
        public string Unsecure(Encoding encoding) => encoding.GetString(Unprotect());
#endif
        public void Append(char c) => InsertAt(-1, c);

        private byte[] Unprotect() => ProtectedData.Unprotect(_protected, _entropy, DataProtectionScope.CurrentUser);    }
}