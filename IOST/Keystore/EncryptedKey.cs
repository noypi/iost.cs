namespace IOSTSdk.Keystore
{
    using IOSTSdk.Crypto;
    using System;

    public class EncryptedKey
    {
        public string Algo { get; set; }
        public string Label { get; set; }
        public string Cipher { get; set; }
        public string Nonce { get; set; }

        public SecureBytes Decrypt(SecureBytes password)
        {
            var cipher = Convert.FromBase64String(Cipher);
            var nonce = Convert.FromBase64String(Nonce);

            if ((string.IsNullOrEmpty(Algo)) ||
                (Algo == "xchacha20-poly1305"))
            {
                return new SecureBytes(SodiumXChaCha20Poly1305.Decrypt(password, cipher, nonce));
            }

            throw new InvalidOperationException($"Unsupported algo: {Algo}");
        }

        public static EncryptedKey EncryptDefault(SecureBytes password, SecureBytes secretKey)
        {
            var enckey = new EncryptedKey { };
            secretKey.UseUnprotected(pk =>
            {
                var (cipher, nonce) = SodiumXChaCha20Poly1305.Encrypt(password, pk);
                enckey.Cipher = Convert.ToBase64String(cipher);
                enckey.Nonce = Convert.ToBase64String(nonce);
            });

            return enckey;
        }
    }
}
