namespace IOSTSdk.Crypto
{
    public class Ed25519 : IAlgorithm
    {
        public byte[] GetPubkey(SecureBytes seckey) => IOST.CryptoGetPubkeyEd25519(seckey);

        public byte[] Sign(byte[] data, SecureBytes seckey) => IOST.CryptoSignEd25519(data, seckey);

        public SecureBytes GeneratePrivateKey() => IOST.CryptoGeneratePrivateKeyEd25519(IOST.CryptoRandomSeed(32));

        public Rpcpb.Signature.Types.Algorithm AlgorithmType => Rpcpb.Signature.Types.Algorithm.Ed25519;
    }
}
