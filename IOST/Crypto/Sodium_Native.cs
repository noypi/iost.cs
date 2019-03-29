namespace IOSTSdk.Crypto
{
    using System.Runtime.InteropServices;

#pragma warning disable IDE1006 // Naming Styles
    public static class Sodium
    {
        private const string LIBSODIUM = "libsodium";

        static Sodium()
        {
            // https://download.libsodium.org/doc/advanced/scrypt#notes
            Sodium.sodium_init();
        }

        [DllImport(LIBSODIUM, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sodium_init();

        [DllImport(LIBSODIUM, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void randombytes_buf(byte[] buffer, int size);

        [DllImport(LIBSODIUM, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int crypto_sign_seed_keypair(byte[] publicKey, byte[] secretKey, byte[] seed);

        [DllImport(LIBSODIUM, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int crypto_sign_ed25519_sk_to_pk(byte[] publicKey, byte[] secretKey);

        [DllImport(LIBSODIUM, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int crypto_sign_verify_detached(byte[] signature, byte[] message, long messageLength, byte[] key);

        [DllImport(LIBSODIUM, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int crypto_sign_detached(byte[] signature, ref long signatureLength, byte[] message, long messageLength, byte[] key);

        [DllImport(LIBSODIUM, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int crypto_aead_xchacha20poly1305_ietf_encrypt(
                                                                        byte[] cipher, out long cipherLength,
                                                                        byte[] message, long messageLength,
                                                                        byte[] additionalData, long additionalDataLength,
                                                                        byte[] unused, byte[] publicNonce,
                                                                        byte[] secret);

        [DllImport(LIBSODIUM, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int crypto_aead_xchacha20poly1305_ietf_decrypt(
                                                                        byte[] message, out long messageLength,
                                                                        byte[] unused,
                                                                        byte[] cipher, long cipherLength,
                                                                        byte[] additionalData, long additionalLength,
                                                                        byte[] publicNonce, byte[] secret);
    }
#pragma warning restore IDE1006 // Naming Styles
}
