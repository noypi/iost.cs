namespace IOST.Crypto
{
    public interface IAlgorithm
    {
        byte[] Sign(byte[] data, byte[] key);
        byte[] GetPubkey(byte[] seckey);

        Rpcpb.Signature.Types.Algorithm AlgorithmType { get; }
    }
}
