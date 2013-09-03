using System;

namespace GTFSimple.Core
{
    public static class Extensions
    {
        public static int? ConvertToInt32(this string s)
        {
            int result;
            return int.TryParse(s, out result) ? result : default(int?);
        }

        public static TimeSpan? ConvertToTimeSpan(this string s)
        {
            TimeSpan result;
            return TimeSpan.TryParse(s, out result) ? result : default(TimeSpan?);
        }
    }
}
