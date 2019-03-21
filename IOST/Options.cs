using IOST.Helpers;
using System;

namespace IOST
{
    public class Options
    {
        public long GasLimit { get; set; } = 1000000;
        public long GasRatio { get; set; } = 1;
        public long ExpirationInMillis { get; set; } = 90000;
        public long Delay { get; set; } = 0;
        public uint ChainId { get; set; } = 1024;
    }
}