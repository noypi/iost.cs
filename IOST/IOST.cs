namespace IOST
{
    using System;
    using System.Security.Cryptography;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class IOST
    {
        /// <summary>
        /// Used to serialize objects to JSON string
        /// </summary>
        public static Func<object, string> JSONSerializer { get; set; } = JsonConvert.SerializeObject;

        /// <summary>
        /// Used to roundoff the value of coins
        /// </summary>
        public static Func<double, double> MathRound { get; set; } = d => d;

        /// <summary>
        /// Hash function used to sign messages
        /// </summary>
        public static Func<byte[], byte[]> CryptoHashSha3_256 { get; set; } = (data) => SHA3.Net.Sha3.Sha3256().ComputeHash(data);

        /// <summary>
        /// Ed225519 sign implementation
        /// </summary>
        public static Func<byte[], byte[], byte[]> CryptoSignEd25519 { get; set; } = Sodium.PublicKeyAuth.Sign;

        /// <summary>
        /// Secp256k1 sign implementation
        /// </summary>
        public static Func<byte[], byte[], byte[]> CryptoSignSecp256k1 { get; set; } = Cryptography.ECDSA.Secp256K1Manager.SignCompressedCompact;

        /// <summary>
        /// Get Ed25519 public key
        /// </summary>
        public static Func<byte[], byte[]> CryptoGetPubkeyEd25519 { get; set; } = Sodium.PublicKeyAuth.ExtractEd25519PublicKeyFromEd25519SecretKey;

        /// <summary>
        /// Get Secp256k1 public key
        /// </summary>
        public static Func<byte[], byte[]> CryptoGetPubkeySecp256k1 { get; set; } = (seckey) => Cryptography.ECDSA.Secp256K1Manager.GetPublicKey(seckey, false);

        /// <summary>
        /// Get Secp256k1 public key (compressed)
        /// </summary>
        public static Func<byte[], byte[]> CryptoGetPubkeySecp256k1Compressed { get; set; } = (seckey) => Cryptography.ECDSA.Secp256K1Manager.GetPublicKey(seckey, true);

        /// <summary>
        /// Generate Ed25519 private key
        /// </summary>
        public static Func<byte[], byte[]> CryptoGeneratePrivateKeyEd25519 { get; set; } = (seed) => Sodium.PublicKeyAuth.GenerateKeyPair(seed).PrivateKey;

        /// <summary>
        /// Generate Secp256k1 private key
        /// </summary>
        public static Func<byte[], byte[]> CryptoGeneratePrivateKeySecp256k1 { get; set; } = (seed) => Cryptography.ECDSA.Secp256K1Manager.GenerateRandomKey();

        /// <summary>
        /// Get seed
        /// </summary>
        public static Func<int, byte[]> CryptoRandomSeed { get; set; } = (n) => Sodium.SodiumCore.GetRandomBytes(n);

        /// <summary>
        /// Base58 decoder
        /// </summary>
        public static Func<string, byte[]> Base58Decode { get; set; } = Cryptography.ECDSA.Base58.Decode;

        /// <summary>
        /// Base58 encoder
        /// </summary>
        public static Func<byte[], string> Base58Encode { get; set; } = Cryptography.ECDSA.Base58.Encode;

        /// <summary>
        /// Configure gas limit, gas ratio, delay, expiration
        /// </summary>
        public Options Options { get; private set; }

        private readonly TxBuilder _txBuilder;

        private Client _client;

        /// <summary>
        /// Create a new IOST
        /// </summary>
        /// <param name="client"></param>
        /// <param name="options"></param>
        public IOST(Client client, Options options)
        {
            Options = options;
            _txBuilder = new TxBuilder(options);
            _client = client;
        }

        /// <summary>
        /// Used to create common transactions
        /// </summary>
        /// <returns></returns>
        public TxBuilder CreateTx() => _txBuilder;

        /// <summary>
        /// Sends the transaction
        /// </summary>
        /// <param name="tx"></param>
        /// <param name="keychain"></param>
        /// <returns>the transaction hash</returns>
        public Task<string> Send(Transaction tx, Keychain keychain, params string[] perm)
        {
            foreach (var s in perm)
            {
                keychain.Sign(tx, s);
            }

            return _client.SendTransaction(tx.TransactionRequest)
                          .ContinueWith<string>((task) => 
                           {
                               if (task.Exception != null)
                               {
                                   throw task.Exception;
                               }
                               return task.Result?.Hash;
                           });
        }
    }
}
