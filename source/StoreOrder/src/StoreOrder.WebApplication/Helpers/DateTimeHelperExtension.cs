﻿using System;

namespace StoreOrder.WebApplication.Helpers
{
    public static class DateTimeHelperExtension
    {
        public static DateTime FromDateTimeOffset(this DateTimeOffset utcTimeOffset)
        {
            return utcTimeOffset.DateTime;
        }

        public static DateTimeOffset ToDateTimeOffset(this DateTime utcTime)
        {
            return DateTime.SpecifyKind(utcTime, DateTimeKind.Utc);
        }
    }
}
