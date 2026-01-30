using Apino.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Domain.Entities
{
    public class OrderStatus : BaseEntity
    {
        public long OrderId { get; set; }
        public long StatusTypeId { get; set; }
        public long PaymentTypeId { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreationDateTime { get; set; }

        public Order Order { get; set; }
        public OrderStatusType StatusType { get; set; }
    }

}
