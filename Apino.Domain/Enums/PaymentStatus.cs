using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Domain.Enums
{
    public enum PaymentStatus
    {
        Pending = 1,   // در انتظار پرداخت
        Success = 2,   // موفق
        VerifyByBranch = 3,     // تایید شعبه.
        Preparing = 4,     // در حال آماده‌سازی.
        Done = 5,     // تحویل شده.
        Canceled = 6,     // لغو شده.
        Draft = 7,     // در سبد خرید.
    }
}
