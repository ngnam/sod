using System;

namespace StoreOrder.WebApplication.Helpers
{
    public partial class DateTimeHelper
    {
        public static DateTime FromDateTimeOffset(DateTimeOffset utcTimeOffset)
        {
            return utcTimeOffset.DateTime;
        }

        public static DateTimeOffset ToDateTimeOffset(DateTime utcTime)
        {
            return DateTime.SpecifyKind(utcTime, DateTimeKind.Utc);
        }
    }
}
