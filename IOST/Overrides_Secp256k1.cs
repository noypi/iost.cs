namespace IOST
{
    using System;
    using System.Text;

    public partial class IOST
    {
        /// <summary>
        /// Secp256k1 sign implementation
        /// </summary>
        public static Func<byte[], Crypto.SecureBytes, byte[]> CryptoSignSecp256k1 { get; set; } = (data, seckey) =>
        {
            byte[] signed = null;
            seckey.UseUnprotected(privkey => signed = Cryptography.ECDSA.Secp256K1Manager.SignCompressedCompact(data, privkey));
            return signed;
        };

        /// <summary>
        /// Get Secp256k1 public key
        /// </summary>
        public static Func<byte[], byte[]> CryptoGetPubkeySecp256k1 { get; set; } = (seckey) => Cryptography.ECDSA.Secp256K1Manager.GetPublicKey(seckey, false);

        /// <summary>
        /// Get Secp256k1 public key (compressed)
        /// </summary>
        public static Func<Crypto.SecureBytes, byte[]> CryptoGetPubkeySecp256k1Compressed { get; set; } = (seckey) =>
        {
            byte[] pubkey = null;
            seckey.UseUnprotected(privkey => pubkey = Cryptography.ECDSA.Secp256K1Manager.GetPublicKey(privkey, true));
            return pubkey;
        };

        /// <summary>
        /// Generate Secp256k1 private key
        /// </summary>
        public static Func<byte[], byte[]> CryptoGeneratePrivateKeySecp256k1 { get; set; } = (seed) => Cryptography.ECDSA.Secp256K1Manager.GenerateRandomKey();

    }
}
