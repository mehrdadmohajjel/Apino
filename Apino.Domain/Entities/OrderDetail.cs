using Apino.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Domain.Entities
{
    public class OrderDetail : BaseEntity
    {
        public long OrderId { get; set; }
        public long ProductId { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }   

    }

}
