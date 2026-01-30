using Apino.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Domain.Entities
{
    public class CartItem : BaseEntity
    {
        public long CartId { get; set; }
        public long ProductId { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
        // از ProductCategory می‌آید
        public bool PayAtPlace { get; set; }

        public Cart Cart { get; set; }
        public Product Product { get; set; }
    }

}
