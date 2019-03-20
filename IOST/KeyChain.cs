namespace IOST
{
    using System.Collections.Generic;
    using global::IOST.Crypto;

    public class Keychain
    {
        public string _ID { get; internal set; }

        private static readonly IAlgorithm _Ed25519 = new Crypto.Ed25519();
        private static readonly IAlgorithm _Secp256k1 = new Crypto.Secp256k1();

        private Dictionary<string, KeyPair> _keys = new Dictionary<string, KeyPair>();

        public Keychain(string id)
        {
            _ID = id;
        }

        public void AddKey(byte[] seckey, params string[] perm)
        {
            IAlgorithm algo = (seckey.Length == 64) ? _Ed25519 : _Secp256k1;

            var keypair = new KeyPair(new SecureBytes(seckey), algo);
            foreach(var p in perm)
            {
                _keys[p] = keypair;
            }

            if (!_keys.ContainsKey("active"))
            {
                _keys["active"] = keypair;
            }
        }

        internal void Sign(Transaction tx)
        {
            var txRequest = tx.TransactionRequest;
            var txBytes = tx.BytesForSigning();
            txRequest.Publisher = _ID;

            var kp = _keys["active"];
            var sig = kp.Sign(IOST.CryptoHashSha3_256(txBytes));

            var sigPb = new Rpcpb.Signature();
            sigPb.PublicKey = Google.Protobuf.ByteString.CopyFrom(kp.PubKey, 0, kp.PubKey.Length);
            sigPb.Algorithm = kp.Algo.AlgorithmType;
            sigPb.Signature_ = Google.Protobuf.ByteString.CopyFrom(sig, 0, sig.Length);

            txRequest.PublisherSigs.Add(sigPb);
        }
    }
}
