namespace IOSTSdk.Keystore
{
    public class EncryptedKey
    {
        public string Algo { get; set; }
        public string Label { get; set; }
        public string Cipher { get; set; }
        public string Nonce { get; set; }
    }
}
