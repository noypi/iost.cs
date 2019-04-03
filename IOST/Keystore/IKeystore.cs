namespace IOSTSdk.Keystore
{
    using IOSTSdk.Crypto;

    public interface IKeystore
    {
        void AddKey(SecureBytes privateKey, SecureBytes password, string label);
        void DeleteKey(string label, EncryptedKey enckey);
        void Store();
        void ClearCredentials();
        void Use(EncryptedKey enckey, SecureBytes password);
        EncryptedKey[] EncryptedKeys { get; }
    }
}
