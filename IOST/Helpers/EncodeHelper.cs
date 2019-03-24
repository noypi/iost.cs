namespace IOSTSdk.Helpers
{
    using System;
    using System.Linq;

    public class EncodeHelper
    {
        public static byte[] ToBytes(long n)
        {
            var bb = EnsureBigEndian( 
                        BitConverter.GetBytes(n));
            return bb;
        }

        public static byte[] ToBytes(uint n)
        {
            var bb = EnsureBigEndian(
                        BitConverter.GetBytes(n));
            return bb;
        }

        public static byte[] ToBytes(int n)
        {
            return EnsureBigEndian(
                        BitConverter.GetBytes(n));
        }

        public static byte[] ToBytes(double n)
        {
            var bb = EnsureBigEndian(
                        BitConverter.GetBytes(n));
            return bb;
        }

        public static byte[] ToBytes(bool n)
        {
            return BitConverter.GetBytes(n);
        }

        private static byte[] EnsureBigEndian(byte[] bb)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bb);
            }
            return bb;
        }
    }
}
