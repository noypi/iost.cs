namespace IOSTSdk.Keystore
{
    using IOSTSdk.Crypto;
    using IOSTSdk.Helpers;
    using IOSTSdk.Keystore.v1;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public abstract class AbstractKeystore
    {
        private Dictionary<string, Keychain> _byAccount = new Dictionary<string, Keychain>();
        
        internal abstract void Initialize();

        public abstract void Store(KeystoreModel kc);

        public abstract EncryptedKey[] EncryptedKeys { get; }

        public void Use(EncryptedKey enckey, SecureBytes password)
        {
            var cipher = Convert.FromBase64String(enckey.Cipher);
            var nonce = Convert.FromBase64String(enckey.Nonce);

            var secure = new SecureBytes(SodiumXChaCha20Poly1305.Decrypt(password, cipher, nonce));

            // parse account:key
            secure.UseUnprotected(bb =>
            {
                var s = Encoding.UTF8.GetString(bb);
                var ss = s.Split(':');
                if (ss.Length != 2)
                {
                    throw new InvalidProgramException("invalid EncryptedKey, not enough data");
                }
                var account = ss[0];
                if (!_byAccount.ContainsKey(account))
                {
                    _byAccount[account] = new Keychain(account);
                }
                var key = Convert.FromBase64String(ss[1]);
                if ((key.Length != Crypto.SodiumEd25519.SECRETKEYBYTES) ||
                    (key.Length != 32))//secp
                {
                    throw new InvalidProgramException("invalid EncryptedKey, unknown key.");
                }
                _byAccount[account].AddKey(key, enckey.Label);
                DataHelper.DestroyData(key);
            });
        }

        public void Reset()
        {
            _byAccount.Clear();
            _byAccount = new Dictionary<string, Keychain>();
        }
    }
}
