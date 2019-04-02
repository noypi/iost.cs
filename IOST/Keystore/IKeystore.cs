namespace IOSTSdk.Keystore
{
    using IOSTSdk.Crypto;

    public interface IKeystore
    {
        void AddKey(SecureBytes password, string accountName, string label, SecureBytes privateKey);
        void DeleteKey(string label, EncryptedKey enckey);
        void Store();
        void ClearCredentials();
        void Use(EncryptedKey enckey, SecureBytes password);
        EncryptedKey[] EncryptedKeys { get; }
    }
}
