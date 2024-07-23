namespace CheckDrive.Api.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime ToTashkentTime(this DateTime dateTime)
        {
            TimeZoneInfo tashkentTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Tashkent");
            return TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.Local, tashkentTimeZone);
        }
    }
}
