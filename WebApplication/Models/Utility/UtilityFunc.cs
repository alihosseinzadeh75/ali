using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace WebApplication.Models.Utility
{
    public class UtilityFunc
    {
        public DateTime ToShmasi(DateTime dt)
        {
            PersianCalendar PcDate = new PersianCalendar();
            int Year = PcDate.GetYear(dt);
            int Month = PcDate.GetMonth(dt);
            int Day = PcDate.GetDayOfMonth(dt);

            return new DateTime(Year, Month, Day);
        }
    }
}