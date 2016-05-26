using System;
using System.Threading;

namespace QA.Core.Extensions
{
    public static class DateTimeEx
    {
        /// <summary>
        /// Меняет время у даты
        /// </summary>
        /// <param name="dateTime">Дата</param>
        /// <param name="hours">Новый час</param>
        /// <param name="minutes">Новые минуты</param>
        /// <param name="seconds">Новые секунды</param>
        /// <param name="milliseconds">Новые миллисекунды</param>
        /// <returns></returns>
        public static DateTime ChangeTime(this DateTime dateTime, int hours, int minutes, int seconds, int milliseconds)
        {
            return new System.DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                hours,
                minutes,
                seconds,
                milliseconds,
                dateTime.Kind);
        }

        /// <summary>
        /// Возвращает первый день недели
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="startOfWeek">Дата, для которой необходимо вернуть первый день недели</param>
        /// <returns></returns>
        public static DateTime StartOfWeek(this DateTime dateTime)
        {
            DateTime firstDayInWeek = dateTime.Date;

            var ci = Thread.CurrentThread.CurrentCulture;
            DayOfWeek fdow = ci.DateTimeFormat.FirstDayOfWeek;

            while (firstDayInWeek.DayOfWeek != fdow)
            {
                firstDayInWeek = firstDayInWeek.AddDays(-1);
            }

            return firstDayInWeek;
        }

        /// <summary>
        /// Возвращает последний день недели
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="startOfWeek">Дата, для которой необходимо вернуть последний день недели</param>
        /// <returns></returns>
        public static DateTime EndOfWeek(this DateTime dateTime)
        {
            var firstDay = dateTime.StartOfWeek();
            return firstDay.AddDays(6);
        }
    }
}
