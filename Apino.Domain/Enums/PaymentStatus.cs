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
        Success = 2,
        PaymentFailed =3,// پرداخت ناموفق
        VerifyByBranch = 4,     // تایید شعبه.
        Preparing = 5,     // در حال آماده‌سازی.
        Done = 6,     // تحویل شده.
        Canceled = 7,     // لغو شده.
        Draft = 0,     // در سبد خرید.
    }
}
