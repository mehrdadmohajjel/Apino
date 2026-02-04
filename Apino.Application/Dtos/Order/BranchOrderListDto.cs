using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Dtos.Order
{
    public class BranchOrderListDto
    {
        public long Id { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public decimal TotalAmount { get; set; }
        public string CreateDate { get; set; } // شمسی
        public long StatusId { get; set; }
        public string StatusTitle { get; set; }

        // داده‌های مورد نیاز برای دکمه عملیات
        public long NextStatusId { get; set; }
        public string NextStatusTitle { get; set; }
        public string NextStatusColor { get; set; } // رنگ دکمه (primary, warning, success, ...)
        public bool IsFinished { get; set; }
    }
}
