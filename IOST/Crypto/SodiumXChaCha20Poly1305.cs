namespace IOSTSdk.Crypto
{
    using System;
    using System.Runtime.InteropServices;

    public class SodiumXChaCha20Poly1305
    {
        public static readonly uint NONCEBYTES     = 24;
        public static readonly uint SECRETKEYBYTES = 32;
        public static readonly uint AEADBYTES      = 16;

        /// <summary>
        /// Encrypts a message
        /// </summary>
        /// <param name="password">the password</param>
        /// <param name="message">the message</param>
        /// <returns>(cipher, nonce)</returns>
        public static (byte[], byte[]) Encrypt(SecureBytes password, byte[] message)
        {
            var nonce = IOST.CryptoRandomSeed((int)NONCEBYTES);
            byte[] hash256 = null;
            password.UseUnprotected(key => hash256 = IOST.CryptoHashSha3_256(key));
            var cipher = new byte[message.Length + AEADBYTES];
            long len;
            int result = Sodium.crypto_aead_xchacha20poly1305_ietf_encrypt(
                                            cipher, out len,
                                            message, message.Length,
                                            null, 0,
                                            null, nonce,
                                            hash256);
            if (result != 0)
            {
                throw new InvalidOperationException("unable to encrypt message");
            }

            return (cipher, nonce);
        }

        /// <summary>
        /// Decrypts
        /// </summary>
        /// <param name="password"></param>
        /// <param name="cipher"></param>
        /// <param name="nonce"></param>
        /// <returns>plain text</returns>
        public static byte[] Decrypt(SecureBytes password, byte[] cipher, byte[] nonce)
        {
            byte[] message = new byte[cipher.Length - AEADBYTES];
            byte[] hash256 = null;
            password.UseUnprotected(key => hash256 = IOST.CryptoHashSha3_256(key));
            long len;
            int result = Sodium.crypto_aead_xchacha20poly1305_ietf_decrypt(
                                            message, out len,
                                            null,
                                            cipher, cipher.Length,
                                            null, 0,
                                            nonce, hash256);
            if (result != 0)
            {
                throw new InvalidOperationException("unable to encrypt message");
            }

            return message;
        }
    }
}
