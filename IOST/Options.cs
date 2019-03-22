using IOST.Helpers;
using System;

namespace IOST
{
    public class Options
    {
        public double GasLimit { get; set; } = 1000000;
        public double GasRatio { get; set; } = 1;
        public long ExpirationInMillis { get; set; } = 90000;
        public long Delay { get; set; } = 0;
        public uint ChainId { get; set; } = 1024;
    }
}