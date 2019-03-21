namespace IOST
{
    using System;
    using System.Collections.Generic;
    using global::IOST.Crypto;
    using Protobuf = Google.Protobuf;

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
        }

        internal void Sign(Transaction tx, string perm)
        {
            if (!_keys.ContainsKey(perm))
            {
                throw new ArgumentException($"perm: '{perm}' not found.");
            }

            var txRequest = tx.TransactionRequest;
            var txBytes = tx.BytesForSigning();
            txRequest.Publisher = _ID;

            var kp = _keys[perm];
            var sig = kp.Sign(IOST.CryptoHashSha3_256(txBytes));

            var sigPb = new Rpcpb.Signature()
            {
                PublicKey = Protobuf.ByteString.CopyFrom(kp.PubKey, 0, kp.PubKey.Length),
                Algorithm = kp.Algo.AlgorithmType,
                Signature_ = Protobuf.ByteString.CopyFrom(sig, 0, sig.Length)
            };

            txRequest.PublisherSigs.Add(sigPb);
        }
    }
}