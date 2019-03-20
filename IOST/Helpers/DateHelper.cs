namespace IOST.Helpers
{
    using System;

    public class DateHelper
    {
        public const int TicksPerMicrosecond = 10;
        public const int NanosecondsPerTick = 100;

        public static long UnixNano()
        {
            DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (DateTime.UtcNow - epochStart).Ticks * NanosecondsPerTick;
        }
    }
}
