using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Dtos.Cart
{
    public class UpdateCartQtyRequest
    {
        public long ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
