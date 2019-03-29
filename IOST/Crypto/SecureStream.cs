namespace IOSTSdk.Crypto
{
    using IOSTSdk.Crypto;
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public class SecureStream : IDisposable
    {
        private ICryptoTransform _encryptor;
        private MemoryStream _tempmem;
        private Rijndael _alg;

        public SecureStream()
        {
            _alg = Rijndael.Create();
            _encryptor = _alg.CreateEncryptor();
            _tempmem = new MemoryStream();
            Writer = new CryptoStream(_tempmem, _encryptor, CryptoStreamMode.Write);
        }

        public CryptoStream Writer { get; private set; }

        public void Append(char s)
        {
            Writer.WriteByte(Convert.ToByte(s));
        }

        public SecureBytes ToSecureBytes()
        {
            Writer.Flush();

            var decryptor = _alg.CreateDecryptor();
            var encrypted = _tempmem.ToArray();
            using (var mem = new MemoryStream(encrypted))
            using (var cryptReader = new CryptoStream(mem, decryptor, CryptoStreamMode.Read))
            using (var rdr = new StreamReader(cryptReader))
            using (var outmem = new MemoryStream())
            {
                cryptReader.CopyTo(outmem);
                var plain = outmem.ToArray();
                return new SecureBytes(plain);
            }
            
        }

        public void Dispose()
        {
            Writer?.Dispose();
            Writer = null;

            _tempmem?.Dispose();
            _tempmem = null;

            _alg?.Dispose();
            _alg = null;
        }
    }
}
