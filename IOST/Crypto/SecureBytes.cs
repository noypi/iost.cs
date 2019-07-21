namespace IOSTSdk.Crypto
{
    using System;

    public class SecureBytes : ISecureBytes
    {
        private readonly ISecureBytes _secureBytes;

        public SecureBytes(byte[] data)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                _secureBytes = new SecureBytesWin(data);
            }
            else
            {
                _secureBytes = new SecureBytesMobile(data);
            }
        }

        public void UseUnprotected(Action<byte[]> action)
        {
            _secureBytes.UseUnprotected(action);
        }
    }
}
