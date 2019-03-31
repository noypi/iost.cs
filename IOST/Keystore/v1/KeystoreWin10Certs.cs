using IOSTSdk.Crypto;

namespace IOSTSdk.Keystore.v1
{
    public class KeystoreWin10Certs : AbstractKeystore
    {
        public override EncryptedKey[] EncryptedKeys => throw new System.NotImplementedException();

        internal KeystoreWin10Certs(Options opts)
        {
        }

        internal override void Initialize()
        {
            throw new System.NotImplementedException();
        }

        public override void AddKey(SecureBytes password, string accountName, string label, SecureBytes privateKey)
        {
            throw new System.NotImplementedException();
        }

        public override void Store()
        {
            throw new System.NotImplementedException();
        }

        public override void DeleteKey(string label, EncryptedKey enckey)
        {
            throw new System.NotImplementedException();
        }
    }
}
