namespace IOSTSdk.Crypto
{
    using System.Runtime.InteropServices;

#pragma warning disable IDE1006 // Naming Styles
    public partial class SodiumEd25519
    {
        private const string LIBSODIUM = "libsodium";

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

    }
#pragma warning restore IDE1006 // Naming Styles
}
