namespace IOSTSdk.Keystore.v1
{
    public class KeystoreModel
    {
        public string Version { get; set; }
        public EncryptedKey[] Keys { get; set; }
    }
}
