using IOSTSdk.Crypto;
using IOSTSdk.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace IOSTSdk.Test
{
    [TestClass]
    public class GoAndCsharpTests
    {
        [TestMethod]
        public async Task TestFloat64ToBytes()
        {
            double val = 1234.58961234;

            // expected result from go's binary.BigEndian.PutUint64()
            byte[] expected = new byte[] { 64, 147, 74, 91, 195, 86, 86, 121 };

            var bb = EncodeHelper.ToBytes(val);
            CollectionAssert.AreEqual(expected, bb);
        }

        [TestMethod]
        public async Task TestLongToBytes()
        {
            long val = 1234901027487210924;

            // expected result from go's binary.BigEndian.PutUint64()
            byte[] expected = new byte[] { 17, 35, 63, 241, 20, 35, 221, 172 };

            var bb = EncodeHelper.ToBytes(val);
            CollectionAssert.AreEqual(expected, bb);
        }
    }
}
