namespace IOST.Helpers
{
    using System;

    public class DateHelper
    {
        public const int NanosecondsPerTick = 100;

        public static long UnixNano()
        {
            DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return TicksToNanoseconds((DateTime.UtcNow - epochStart).Ticks);
        }

        public static long TicksToNanoseconds(long ticks) => ticks * NanosecondsPerTick;

        public static double TicksToMilliseconds(long ticks) => new TimeSpan(ticks).TotalMilliseconds;
    }
}
