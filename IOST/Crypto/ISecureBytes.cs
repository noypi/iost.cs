using System;

namespace IOSTSdk.Crypto
{
    public interface ISecureBytes
    {
        void UseUnprotected(Action<byte[]> action);
    }
}
