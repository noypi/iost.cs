namespace IOSTSdk.Crypto
{
    public class Secp256k1 : IAlgorithm
    {
        public byte[] GetPubkey(SecureBytes seckey) => IOST.CryptoGetPubkeySecp256k1Compressed(seckey);

        public byte[] Sign(byte[] data, SecureBytes seckey) => IOST.CryptoSignSecp256k1(data, seckey);

        public SecureBytes GeneratePrivateKey() => throw new System.NotImplementedException("Generation of Secp256k1 is not supported");

        public Rpcpb.Signature.Types.Algorithm AlgorithmType => Rpcpb.Signature.Types.Algorithm.Secp256K1;
    }
}
