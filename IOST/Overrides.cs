namespace IOSTSdk
{
    using System;
    using System.IO;
    using System.Text;

    public partial class IOST
    {
        /// <summary>
        /// Used to serialize objects to JSON string
        /// </summary>
        public static Func<object, string> JSONSerializer { get; set; } = Newtonsoft.Json.JsonConvert.SerializeObject;

        /// <summary>
        /// Used to roundoff the value of coins
        /// </summary>
        public static Func<double, double> MathRound { get; set; } = d => Math.Round(d, 8);

        /// <summary>
        /// Hash function used to sign messages
        /// </summary>
        public static Func<byte[], byte[]> CryptoHashSha3_256 { get; set; } = (data) => SHA3.Net.Sha3.Sha3256().ComputeHash(data);

        /// <summary>
        /// Ed225519 sign implementation
        /// </summary>
        public static Func<byte[], Crypto.SecureBytes, byte[]> CryptoSignEd25519 { get; set; } = Crypto.SodiumEd25519.SignDetached;

        /// <summary>
        /// Ed225519 verify
        /// </summary>
        public static Func<byte[], byte[], byte[], bool> CryptoVerifyEd25519 { get; set; } = Crypto.SodiumEd25519.VerifyDetached;
        
        /// <summary>
        /// Get Ed25519 public key
        /// </summary>
        public static Func<Crypto.SecureBytes, byte[]> CryptoGetPubkeyEd25519 { get; set; } = Crypto.SodiumEd25519.ExtractPublicKey;

        /// <summary>
        /// Generate Ed25519 private key
        /// </summary>
        public static Func<byte[], Crypto.SecureBytes> CryptoGeneratePrivateKeyEd25519 { get; set; } = (seed) => Crypto.SodiumEd25519.NewKeyPair(seed).PrivKey;
        
        /// <summary>
        /// Get seed
        /// </summary>
        public static Func<int, byte[]> CryptoRandomSeed { get; set; } = Crypto.SodiumEd25519.RandomBytes;

        /// <summary>
        /// Base58 decoder
        /// </summary>
        public static Func<string, byte[]> Base58Decode { get; set; } = Crypto.Base58Encoding.Decode;

        /// <summary>
        /// Base58 encoder
        /// </summary>
        public static Func<byte[], string> Base58Encode { get; set; } = Crypto.Base58Encoding.Encode;

        /// <summary>
        /// Use UTF8 for protobuf strings
        /// See: https://developers.google.com/protocol-buffers/docs/proto3#scalar
        /// </summary>
        public static Encoding DefaultTextEncoding { get; set; } = Encoding.UTF8;
    }
}
