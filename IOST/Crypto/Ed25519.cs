namespace IOST.Crypto
{
    public class Ed25519 : IAlgorithm
    {
        public byte[] GetPubkey(byte[] seckey) => IOST.CryptoGetPubkeyEd25519(seckey);

        public byte[] Sign(byte[] data, byte[] key) => IOST.CryptoSignEd25519(data, key);

        public byte[] GeneratePrivateKey() => IOST.CryptoGeneratePrivateKeyEd25519(IOST.CryptoRandomSeed(32));

        public Rpcpb.Signature.Types.Algorithm AlgorithmType => Rpcpb.Signature.Types.Algorithm.Ed25519;
    }
}
