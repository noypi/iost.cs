namespace IOST.Crypto
{
    public class KeyPair
    {
        public SecureBytes PrivKey { get; private set; }
        public byte[] PubKey { get; private set; }
        public IAlgorithm Algo { get; private set; }

        public KeyPair(SecureBytes seckey, IAlgorithm algo)
        {
            Algo = algo;
            seckey.UseUnprotected(privk => PubKey = algo.GetPubkey(privk));
            PrivKey = seckey;
        }

        public byte[] Sign(byte[] data)
        {
            byte[] result = null;
            PrivKey.UseUnprotected(privk => result = Algo.Sign(data, privk));
            return result;
        }
    }
}
