using IOST.Crypto;
using IOST.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Threading.Tasks;

namespace IOST.Test
{
    /// <summary>
    /// https://github.com/iost-official/go-iost/blob/6958d128390fecd99cd36a90692a7747fc8c0f57/common/serialize_test.go
    /// </summary>
    [TestClass]
    public class SimpleEncoderTests
    {
        [TestMethod]
        public void TestWriteBytes()
        {
            var se = new SimpleEncoder(1024);
            se.TextEncoding = Encoding.ASCII;
            se.Put(new byte[] { (byte)'a', (byte)'a', (byte)'a' });

            var expected = new byte[] { 0, 0, 0, 0x3, (byte)'a', (byte)'a', (byte)'a' };
            var actual = se.GetBytes();
            CollectionAssert.AreEqual(expected, actual);

            se.Put(new byte[] { (byte)'b', (byte)'b' });
            expected = new byte[] { 0, 0, 0, 0x3, (byte)'a', (byte)'a', (byte)'a',
                                    0, 0, 0, 0x2, (byte)'b', (byte)'b' };

            CollectionAssert.AreEqual(expected, se.GetBytes());
        }

        [TestMethod]
        public void TestWriteString()
        {
            var se = new SimpleEncoder(1024);
            se.TextEncoding = Encoding.ASCII;
            se.Put("aaa");
            var actual = se.GetBytes();
            var expected = new byte[] { 0, 0, 0, 0x3, (byte)'a', (byte)'a', (byte)'a' };
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestWriteLong()
        {
            var se = new SimpleEncoder(1024);
            se.Put(1023);
            var expected = new byte[] { 0, 0, 0, 0, 0, 0, 0x3, 0xff };
            CollectionAssert.AreEqual(expected, se.GetBytes());
        }

        [TestMethod]
        public void TestWriteBytesSlice()
        {
            var se = new SimpleEncoder(1024);
            se.TextEncoding = Encoding.ASCII;
            var data = new byte[][]{ new byte[] { (byte)'a', (byte)'a' } , new byte[]{ (byte)'b', (byte)'b' } };
            se.Put(data);
            var expected = new byte[] { 0, 0, 0, 0x2, 0, 0, 0, 0x2, (byte)'a', (byte)'a', 0, 0, 0, 0x2, (byte)'b', (byte)'b' };
            CollectionAssert.AreEqual(expected, se.GetBytes());
        }

        [TestMethod]
        public void TestWriteStringSlice()
        {
            var se = new SimpleEncoder(1024);
            se.TextEncoding = Encoding.ASCII;
            var data = new string[] { "aa", "bb" };
            se.Put(data);
            var expected = new byte[] { 0, 0, 0, 0x2, 0, 0, 0, 0x2, (byte)'a', (byte)'a', 0, 0, 0, 0x2, (byte)'b', (byte)'b' };
            CollectionAssert.AreEqual(expected, se.GetBytes());
        }
    }
}
