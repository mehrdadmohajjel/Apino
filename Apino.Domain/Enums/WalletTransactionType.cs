using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Domain.Enums
{
    public enum WalletTransactionType
    {
        Deposit = 1,   // افزایش موجودی
        Withdraw = 2,  // برداشت
        Refund = 3     // بازگشت وجه
    }
}
