using Apino.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apino.Domain.Enums;

namespace Apino.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public long OrderId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; } // Online, Wallet, PayAtPlace
        public PaymentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
