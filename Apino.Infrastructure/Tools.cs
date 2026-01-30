using Apino.Application.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Infrastructure
{
    public class Tools : IToolsService
    {
        public string GenerateOrderNumber()
        {
            PersianCalendar persianCalendar = new PersianCalendar();
            // استخراج اجزای تاریخ شمسی
            int year = persianCalendar.GetYear(DateTime.Now);
            int month = persianCalendar.GetMonth(DateTime.Now);
            int day = persianCalendar.GetDayOfMonth(DateTime.Now);
            var datePart = year.ToString() + month.ToString();
            var randomPart = Guid.NewGuid().ToString("N").Substring(0, 3).ToUpper();
            return  $"APINO-{datePart}-{randomPart}";
        }
    }
}
