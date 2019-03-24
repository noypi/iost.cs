namespace IOSTSdk.Crypto
{
    public class KeyPair
    {
        public SecureBytes PrivKey { get; private set; }
        public byte[] PubKey { get; private set; }
        public IAlgorithm Algo { get; private set; }

        public KeyPair(SecureBytes seckey, IAlgorithm algo)
        {
            Algo = algo;
            PubKey = algo.GetPubkey(seckey);
            PrivKey = seckey;
        }

        public byte[] Sign(byte[] data) => Algo.Sign(data, PrivKey);
    }
}
