using IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeZoneConverter;

namespace Services
{
    public class TimeZoneService : ITimeZoneService
    {
        public DateTime Now()
        {
            DateTime utcTime = DateTime.UtcNow;
            TimeZoneInfo BdZone = TZConvert.GetTimeZoneInfo("Central America Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(utcTime, BdZone);
        }
    }
}
