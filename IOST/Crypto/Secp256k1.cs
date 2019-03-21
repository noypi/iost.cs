namespace IOST.Crypto
{
    public class Secp256k1 : IAlgorithm
    {
        public byte[] GetPubkey(byte[] seckey) => IOST.CryptoGetPubkeySecp256k1Compressed(seckey);

        public byte[] Sign(byte[] data, byte[] key) => IOST.CryptoSignSecp256k1(data, key);

        public Rpcpb.Signature.Types.Algorithm AlgorithmType => Rpcpb.Signature.Types.Algorithm.Secp256K1;
    }
}