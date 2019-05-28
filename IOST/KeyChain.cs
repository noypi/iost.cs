namespace IOSTSdk
{
    using System;
    using System.Collections.Generic;
    using global::IOSTSdk.Crypto;
    using Protobuf = Google.Protobuf;

    public class Keychain
    {
        public string _AccountName { get; internal set; }

        private static readonly IAlgorithm _Ed25519 = new Crypto.Ed25519();
        private static readonly IAlgorithm _Secp256k1 = new Crypto.Secp256k1();

        private Dictionary<string, KeyPair> _keys = new Dictionary<string, KeyPair>();

        public Keychain(string accountName)
        {
            _AccountName = accountName;
        }

        [Obsolete]
        public void AddKey(byte[] privKey, params string[] perm)
        {
            AddKey(new SecureBytes(privKey), perm);
        }

        public void AddKey(SecureBytes seckey, params string[] perm)
        {
            IAlgorithm algo = null;
            seckey.UseUnprotected(pk => algo = (pk.Length == 64) ? _Ed25519 : _Secp256k1);

            var keypair = new KeyPair(seckey, algo);
            foreach(var p in perm)
            {
                _keys[p] = keypair;
            }
        }

        public byte[] GetPublicKey(string perm) => _keys[perm].PubKey;

        public byte[] Sign(string perm, byte[] data) => _keys[perm].Sign(data);

        public void Sign(Transaction tx) => Sign(tx, "active");

        public void Sign(Transaction tx, params string[] perms)
        {
            foreach(var perm in perms)
            {
                SignPerm(tx, perm);
            }
        }

        protected void SignPerm(Transaction tx, string perm)
        {
            if (!_keys.ContainsKey(perm))
            {
                throw new ArgumentException($"perm: '{perm}' not found.");
            }

            var kp = _keys[perm];
            var sig = kp.Sign(GetTxHash(tx));

            var txRequest = tx.TxRequest;
            txRequest.Publisher = _AccountName;
            
            var sigPb = new Rpcpb.Signature()
            {
                PublicKey = Protobuf.ByteString.CopyFrom(kp.PubKey, 0, kp.PubKey.Length),
                Algorithm = kp.Algo.AlgorithmType,
                Signature_ = Protobuf.ByteString.CopyFrom(sig, 0, sig.Length)
            };

            txRequest.PublisherSigs.Add(sigPb);
        }

        protected byte[] GetTxHash(Transaction tx)
        {
            return IOST.CryptoHashSha3_256(tx.BytesForSigning());
        }
    }
}
