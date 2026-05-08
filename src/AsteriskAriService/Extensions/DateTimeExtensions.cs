namespace AsteriskAriService.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Get the current day in the string format year month day 20260506
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string GetDateAsterisk(this DateTime datetime)
        {
            var year = datetime.Year;
            var month = datetime.Month.ToString("00");
            var day = datetime.Day.ToString("00");
            return $"{year}{month}{day}";
        }
        /// <summary>
        /// Get the current day in the string format year month day 2026/05/06/
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string GetDatePathAsterisk(this DateTime datetime)
        {
            var year = datetime.Year;
            var month = datetime.Month.ToString("00");
            var day = datetime.Day.ToString("00");
            return $"{year}/{month}/{day}/";
        }
        /// <summary>
        /// Get the current time in the format of the string hours minutes seconds 132840
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string GetTimeAsterisk(this DateTime datetime)
        {
            var hour = datetime.Hour.ToString("00");
            var minute = datetime.Minute.ToString("00");
            var seconds = datetime.Second.ToString("00");
            return $"{hour}{minute}{seconds}";
        }
    }
}