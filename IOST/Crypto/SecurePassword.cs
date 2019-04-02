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
            char[] cs = { c };
            InsertAt(i, UnicodeEncoding.UTF8.GetBytes(cs));
        }

        public void InsertAt(int i, byte[] bb)
        {
            byte[] unprotected = (_protected != null) ?
                                        Unprotect() :
                                        unprotected = new byte[] { };

            byte[] newdata = new byte[unprotected.Length + bb.Length];
            if (i < 0)
            {
                // do append
                i = unprotected.Length;
            }
            // prepend
            Array.Copy(unprotected, 0, newdata, 0, i);
            // insert data
            Array.Copy(bb, 0, newdata, i, bb.Length);
            // append last part
            Array.Copy(unprotected, i, newdata, i + bb.Length, unprotected.Length - i);

            _protected = ProtectedData.Protect(newdata, _entropy, DataProtectionScope.CurrentUser);
            DataHelper.DestroyData(unprotected);
            DataHelper.DestroyData(newdata);
        }

        public SecureBytes ToSecureBytes()
        {
            var bb = Unprotect();
            var secbb = new SecureBytes(bb);
            DataHelper.DestroyData(bb);
            return secbb;
        }

#if DEBUG
        public string Unsecure(Encoding encoding) => encoding.GetString(Unprotect());
#endif
        public void Append(char c) => InsertAt(-1, c);

        private byte[] Unprotect() => ProtectedData.Unprotect(_protected, _entropy, DataProtectionScope.CurrentUser);    }
}