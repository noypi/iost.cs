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
            throw new NotImplementedException("not implemented in .Net45, override IOST.CryptoSignSecp256k1 manually.");
        };

        /// <summary>
        /// Get Secp256k1 public key
        /// </summary>
        public static Func<byte[], byte[]> CryptoGetPubkeySecp256k1 { get; set; } = 
            (seckey) => throw new NotImplementedException("not implemented in .Net45, override IOST.CryptoGetPubkeySecp256k1 manually.");

        /// <summary>
        /// Get Secp256k1 public key (compressed)
        /// </summary>
        public static Func<Crypto.SecureBytes, byte[]> CryptoGetPubkeySecp256k1Compressed { get; set; } = (seckey) =>
        {
            throw new NotImplementedException("not implemented in .Net45, override IOST.CryptoGetPubkeySecp256k1Compressed manually.");
        };

        /// <summary>
        /// Generate Secp256k1 private key
        /// </summary>
        public static Func<byte[], byte[]> CryptoGeneratePrivateKeySecp256k1 { get; set; } 
            = (seed) => throw new NotImplementedException("not implemented in .Net45, override IOST.CryptoGeneratePrivateKeySecp256k1 manually.");

    }
}
