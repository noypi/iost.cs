namespace IOST.Helpers
{
    using System;
    using System.Linq;

    public class EncodeHelper
    {
        public static byte[] ToBytes(long n)
        {
            return EnsureBigEndian( 
                        BitConverter.GetBytes(n));
        }

        public static byte[] ToBytes(int n)
        {
            return EnsureBigEndian(
                        BitConverter.GetBytes(n));
        }

        public static byte[] ToBytes(double n)
        {
            return EnsureBigEndian(
                        BitConverter.GetBytes(n));
        }

        public static byte[] ToBytes(bool n)
        {
            return EnsureBigEndian(
                        BitConverter.GetBytes(n));
        }

        private static byte[] EnsureBigEndian(byte[] bb)
        {
            if (BitConverter.IsLittleEndian)
            {
                return bb.Reverse().ToArray();
            }
            return bb;
        }
    }
}
