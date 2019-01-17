using System;

namespace MPOWR.Model
{
    public class CommonFunction
    {


        public static DateTime GetCurrentTime
        {
            get
            {
                TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                return TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            }
        }
    }

}
