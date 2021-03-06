namespace IOSTSdk.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class SimpleEncoder : IDisposable
    {
        public Encoding TextEncoding { get; set; } = IOST.DefaultTextEncoding;

        private MemoryStream _buffer = new MemoryStream();

        public SimpleEncoder(int cap)
        {
            _buffer.SetLength(cap);
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

        public SimpleEncoder Put(uint n)
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
            return Put(TextEncoding.GetBytes(s));
        }

        public SimpleEncoder Put(byte[] data)
        {
            Put(data?.Length ?? 0);
            if (0 < data?.Length)
            {
                Append(data);
            }

            return this;
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

        public SimpleEncoder PutList<T>(IList<T> ls, Func<T, Encoding, byte[]> itemToBytes)
        {
            var bb = new byte[ls.Count][];
            for (int i = 0; i < ls.Count; i++)
            {
                bb[i] = itemToBytes(ls[i], TextEncoding);
            }
            Put(bb);

            return this;
        }

        public byte[] GetBytes()
        {
            return _buffer.GetBuffer().Take((int)_buffer.Position).ToArray();
        }

        public void Dispose()
        {
            _buffer.Dispose();
        }
    }
}
