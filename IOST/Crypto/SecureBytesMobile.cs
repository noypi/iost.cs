namespace IOSTSdk.Crypto
{
    using IOSTSdk.Helpers;
    using System;
    using System.Linq;

    public class SecureBytesMobile : ISecureBytes
    {
        private readonly byte[] _entropy;
        private byte[] _protected;
        private byte[] _nonce = null;
        private static byte[] _session = IOST.CryptoRandomSeed(16);

        public SecureBytesMobile(byte[] data)
        {
            _entropy = _session.Concat(IOST.CryptoRandomSeed(16)).ToArray();
            Protect(data);
        }

        public void UseUnprotected(Action<byte[]> action)
        {
            byte[] unprotected = UnProtect();
            action(unprotected);
            DataHelper.DestroyData(unprotected);
        }

        private void Protect(byte[] data)
        {
            _nonce = IOST.CryptoRandomSeed((int)SodiumXChaCha20Poly1305.NONCEBYTES);
            _protected = new byte[data.Length + SodiumXChaCha20Poly1305.AEADBYTES];

            byte[] hash256 = IOST.CryptoHashSha3_256(_entropy);
            long len = 0;
            var result = Sodium.crypto_aead_xchacha20poly1305_ietf_encrypt(
                                            _protected, out len,
                                            data, data.Length,
                                            null, 0,
                                            null, _nonce,
                                            hash256);

            if (result != 0)
            {
                throw new InvalidOperationException("unable to encrypt message");
            }
        }

        private byte[] UnProtect()
        {
            byte[] message = new byte[_protected.Length - SodiumXChaCha20Poly1305.AEADBYTES];
            byte[] hash256 = IOST.CryptoHashSha3_256(_entropy);

            long len = 0;

            var result = Sodium.crypto_aead_xchacha20poly1305_ietf_decrypt(
                                        message, out len,
                                        null,
                                        _protected, _protected.Length,
                                        null, 0,
                                        _nonce, hash256);
            if (result != 0)
            {
                throw new InvalidOperationException("unable to decrypt message");
            }

            return message;
        }
    }
}
