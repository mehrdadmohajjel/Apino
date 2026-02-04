using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Apino.Application.Dtos.Payment
{
    public class PaymentReportItemDto
    {
        public long Id { get; set; }
        public string OrderNumber { get; set; } // برای نمایش شماره سفارش مرتبط
        public string CustomerName { get; set; } // نام مشتری
        public decimal Amount { get; set; }
        public string MethodTitle { get; set; } // آنلاین، نقدی و ...
        public string StatusTitle { get; set; } // موفق، ناموفق
        public int StatusId { get; set; } // برای رنگ‌بندی بج‌ها
        public string CreateDate { get; set; } // تاریخ شمسی
        public string CreateTime { get; set; } // ساعت
        [JsonIgnore]
        public DateTime CreateDateRaw { get; set; }

    }
}
