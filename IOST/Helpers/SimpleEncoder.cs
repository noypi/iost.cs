namespace IOST.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public class SimpleEncoder : IDisposable
    {
        private MemoryStream _buffer = new MemoryStream();

        public SimpleEncoder(int cap)
        {
            _buffer.SetLength(cap);
        }

        public SimpleEncoder(byte[] buf)
        {
            _buffer.SetLength(65536);
        }

        private SimpleEncoder Append(byte[] data)
        {
            _buffer.Write(data, 0, data.Length);
            return this;
        }

        public SimpleEncoder Put(byte b)
        {
            _buffer.WriteByte(b);
            return this;
        }

        public SimpleEncoder Put(long n)
        {
            return Append(EncodeHelper.ToBytes(n));
        }

        public SimpleEncoder Put(double n)
        {
            return Append(EncodeHelper.ToBytes(n));
        }

        public SimpleEncoder Put(int n)
        {
            return Append(EncodeHelper.ToBytes(n));
        }

        public SimpleEncoder Put(String s)
        {
            return Put(UnicodeEncoding.Default.GetByteCount(s))
                        .Append(UnicodeEncoding.Default.GetBytes(s));
        }

        public SimpleEncoder Put(byte[] data)
        {
            return Put(data.Length)
                        .Append(data);
        }

        public SimpleEncoder Put(IList<string> ss)
        {
            Put(ss.Count);
            foreach(var s in ss)
            {
                Put(s);
            }
            return this;
        }

        public SimpleEncoder Put(IList<byte[]> bb)
        {
            Put(bb.Count);
            foreach (var b in bb)
            {
                Put(b);
            }
            return this;
        }

        public byte[] GetBytes()
        {
            return _buffer.GetBuffer();
        }

        public void Dispose()
        {
            _buffer.Dispose();
        }
    }
}
