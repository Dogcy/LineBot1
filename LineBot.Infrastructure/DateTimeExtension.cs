using System;

namespace LineBot.Infrastructure
{
    public static class DateTimeExtension
    {
        public static DateTime TaipeiNow()
    => DateTime.UtcNow.UtcToTaipeiTime();


        /// <summary>
        /// UTC 轉換成台北時間
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime UtcToTaipeiTime(this DateTime time)
            => TimeZoneInfo.ConvertTimeBySystemTimeZoneId(time, "UTC", "Taipei Standard Time");


    }
}
