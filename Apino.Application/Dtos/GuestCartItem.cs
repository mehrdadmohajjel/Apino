using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Dtos
{
    public class GuestCartItem
    {
        public long ProductId { get; set; }
        public long BranchId { get; set; }
        public int Qty { get; set; }
    }

}
