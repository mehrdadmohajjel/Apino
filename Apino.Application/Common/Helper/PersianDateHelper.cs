using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Common.Helper
{
    public static class PersianDateHelper
    {
        // تبدیل میلادی به شمسی (برای نمایش در جدول)
        public static string ToShamsi(DateTime date)
        {
            var pc = new PersianCalendar();
            return $"{pc.GetYear(date)}/{pc.GetMonth(date):00}/{pc.GetDayOfMonth(date):00}";
        }

        // تبدیل شمسی به میلادی (برای جستجو در دیتابیس) -> این متد را کم داشتید
        public static DateTime? ToGregorian(string shamsiDate)
        {
            if (string.IsNullOrWhiteSpace(shamsiDate)) return null;

            // تبدیل اعداد فارسی به انگلیسی (اگر کاربر دستی تایپ کرد)
            shamsiDate = shamsiDate.Replace("۰", "0").Replace("۱", "1").Replace("۲", "2")
                                   .Replace("۳", "3").Replace("۴", "4").Replace("۵", "5")
                                   .Replace("۶", "6").Replace("۷", "7").Replace("۸", "8").Replace("۹", "9");

            var parts = shamsiDate.Split('/');
            if (parts.Length != 3) return null;

            try
            {
                var year = int.Parse(parts[0]);
                var month = int.Parse(parts[1]);
                var day = int.Parse(parts[2]);

                var pc = new PersianCalendar();
                // ساعت را روی 00:00:00 تنظیم می‌کنیم
                return pc.ToDateTime(year, month, day, 0, 0, 0, 0);
            }
            catch
            {
                return null;
            }
        }
    }
}
