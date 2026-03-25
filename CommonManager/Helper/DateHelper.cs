using System;

namespace CommonManager.Helper
{
    /// <summary>
    /// 日期帮助类
    /// </summary>
    public static class DateHelper
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// 转换为 Unix 时间戳（秒）
        /// </summary>
        public static long ToUnixTimestampBySeconds(DateTime dateTime)
        {
            return (long)(dateTime.ToUniversalTime() - UnixEpoch).TotalSeconds;
        }

        /// <summary>
        /// 转换为 Unix 时间戳（毫秒）
        /// </summary>
        public static long ToUnixTimestampByMilliseconds(DateTime dateTime)
        {
            return (long)(dateTime.ToUniversalTime() - UnixEpoch).TotalMilliseconds;
        }

        /// <summary>
        /// Unix 时间戳转换为 DateTime（秒）
        /// </summary>
        public static DateTime FromUnixTimestampBySeconds(long timestamp)
        {
            return UnixEpoch.AddSeconds(timestamp).ToLocalTime();
        }

        /// <summary>
        /// Unix 时间戳转换为 DateTime（毫秒）
        /// </summary>
        public static DateTime FromUnixTimestampByMilliseconds(long timestamp)
        {
            return UnixEpoch.AddMilliseconds(timestamp).ToLocalTime();
        }

        /// <summary>
        /// 获取本周开始时间
        /// </summary>
        public static DateTime GetWeekStart(DateTime dateTime)
        {
            int dayOfWeek = (int)dateTime.DayOfWeek;
            if (dayOfWeek == 0) dayOfWeek = 7;
            return dateTime.Date.AddDays(1 - dayOfWeek);
        }

        /// <summary>
        /// 获取本周结束时间
        /// </summary>
        public static DateTime GetWeekEnd(DateTime dateTime)
        {
            return GetWeekStart(dateTime).AddDays(7).AddMilliseconds(-1);
        }

        /// <summary>
        /// 获取本月开始时间
        /// </summary>
        public static DateTime GetMonthStart(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        /// <summary>
        /// 获取本月结束时间
        /// </summary>
        public static DateTime GetMonthEnd(DateTime dateTime)
        {
            return GetMonthStart(dateTime).AddMonths(1).AddMilliseconds(-1);
        }

        /// <summary>
        /// 格式化日期时间
        /// </summary>
        public static string FormatDateTime(DateTime dateTime, string format = "yyyy-MM-dd HH:mm:ss")
        {
            return dateTime.ToString(format);
        }

        /// <summary>
        /// 计算年龄
        /// </summary>
        public static int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}
