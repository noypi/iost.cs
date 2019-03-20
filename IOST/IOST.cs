namespace IOST
{
    using System;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class IOST
    {
        /// <summary>
        /// Configure gas limit, gas ratio, delay, expiration
        /// </summary>
        public Options Options { get; internal set; }

        /// <summary>
        /// Used to serialize objects to JSON string
        /// </summary>
        public static Func<object, string> JSONSerializer { get; set; } = JsonConvert.SerializeObject;

        /// <summary>
        /// Used to roundoff the value of coins
        /// </summary>
        public static Func<double, double> MathRound { get; set; } = d => d;

        public static Func<byte[], byte[]> CryptoHashSha3_256 { get; set; } = (data) => SHA3.Net.Sha3.Sha3256().ComputeHash(data);

        public static Func<byte[], byte[], byte[]> CryptoSignEd25519 { get; set; } = Sodium.PublicKeyAuth.Sign;

        public static Func<byte[], byte[], byte[]> CryptoSignSecp256k1 { get; set; } = Cryptography.ECDSA.Secp256K1Manager.SignCompressedCompact;

        public static Func<byte[], byte[]> CryptoGetPubkeyEd25519 { get; set; } = Sodium.PublicKeyAuth.ExtractEd25519PublicKeyFromEd25519SecretKey;

        public static Func<byte[], byte[]> CryptoGetPubkeySecp256k1 { get; set; } = (seckey) => Cryptography.ECDSA.Secp256K1Manager.GetPublicKey(seckey, false);

        public static Func<byte[], byte[]> CryptoGetPubkeySecp256k1Compressed { get; set; } = (seckey) => Cryptography.ECDSA.Secp256K1Manager.GetPublicKey(seckey, true);

        private readonly TxBuilder _txBuilder;

        private Client _client;

        public IOST(Client client, Options options)
        {
            Options = options;
            _txBuilder = new TxBuilder(options);
            _client = client;
        }

        public TxBuilder CreateTx() => _txBuilder;

        /// <summary>
        /// Sends the transaction
        /// </summary>
        /// <param name="tx"></param>
        /// <param name="keychain"></param>
        /// <returns>the transaction hash</returns>
        public Task<string> Send(Transaction tx, Keychain keychain)
        {
            keychain.Sign(tx);
            return _client.SendTransaction(tx.TransactionRequest)
                          .ContinueWith<string>((task) => 
                          {
                              task.Wait();
                              return task.Result.Hash;
                          });
        }
    }
}
