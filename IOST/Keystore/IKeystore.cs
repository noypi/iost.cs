namespace IOSTSdk.Keystore
{
    using IOSTSdk.Crypto;

    public interface IKeystore
    {
        void Initialize();
        void AddKey(SecureBytes privateKey, SecureBytes password, string label);
        void RenameKeyLabel(EncryptedKey enckey, string newLabel);
        void DeleteKey(string label, EncryptedKey enckey);
        void Store();
        void ChangePassword(SecureBytes oldPassword, SecureBytes newPassword);
        EncryptedKey ChangeEncryptedKeyPassword(EncryptedKey enckey, SecureBytes oldPassword, SecureBytes newPassword);
        void ClearCredentials();
        void Use(EncryptedKey enckey, SecureBytes password);
        EncryptedKey[] EncryptedKeys { get; }
    }
}
