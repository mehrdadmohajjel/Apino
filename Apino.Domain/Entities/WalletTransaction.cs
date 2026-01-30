using Apino.Domain.Base;
using Apino.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Domain.Entities
{
    public class WalletTransaction : BaseEntity
    {
        public long WalletId { get; set; }
        public decimal Amount { get; set; }
        public WalletTransactionType Type { get; set; } // Charge, Payment, Refund
        public string Reference { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
