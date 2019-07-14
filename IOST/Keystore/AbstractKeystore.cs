namespace IOSTSdk.Keystore
{
    using IOSTSdk.Crypto;
    using IOSTSdk.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public abstract class AbstractKeystore : IKeystore
    {
        private Dictionary<string, Keychain> _byAccount = new Dictionary<string, Keychain>();
        
        public abstract void Initialize();

        public abstract void AddKey(SecureBytes privateKey, SecureBytes password, string label);

        public abstract void RenameKeyLabel(EncryptedKey enckey, string newLabel);

        public abstract void DeleteKey(string label, EncryptedKey enckey);

        public abstract void ChangePassword(SecureBytes oldPassword, SecureBytes newPassword);

        public abstract EncryptedKey ChangeEncryptedKeyPassword(EncryptedKey enckey, SecureBytes oldPassword, SecureBytes newPassword);

        public abstract void Store();

        public abstract EncryptedKey[] EncryptedKeys { get; }

        public void Use(EncryptedKey enckey, SecureBytes password)
        {
            var secure = enckey.Decrypt(password);

            // parse account:key
            secure.UseUnprotected(bb =>
            {
                var s = Encoding.UTF8.GetString(bb);
                var ss = s.Split(':');
                if (ss.Length != 2)
                {
                    throw new InvalidProgramException("Invalid EncryptedKey, not enough data");
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
                    throw new InvalidProgramException("Invalid EncryptedKey, unknown key.");
                }
                _byAccount[account].AddKey(new SecureBytes(key), enckey.Label);
                DataHelper.DestroyData(key);
            });
        }

        public void ClearCredentials()
        {
            _byAccount.Clear();
            _byAccount = new Dictionary<string, Keychain>();
        }
    }
}
