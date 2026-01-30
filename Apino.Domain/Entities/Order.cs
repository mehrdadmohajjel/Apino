using Apino.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Domain.Entities
{
    public class Order : BaseEntity
    {
        public long UserId { get; set; }
        public long BranchId { get; set; } // 🔴 خیلی مهم
        public string OrderNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public long CurrentStatusTypeId { get; set; } // 🔴
        public string PaymentTransactionCode { get; set; }
        public decimal SubTotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public User User { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<OrderStatus> Statuses { get; set; }
    }
}
