namespace IOSTSdk.Crypto
{
    public interface IAlgorithm
    {
        byte[] Sign(byte[] data, SecureBytes seckey);
        byte[] GetPubkey(SecureBytes seckey);
        SecureBytes GeneratePrivateKey();

        Rpcpb.Signature.Types.Algorithm AlgorithmType { get; }
    }
}
