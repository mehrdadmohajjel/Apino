using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Dtos.Payment
{
    public class PayViewModel
    {
        public long OrderId { get; set; }
        public decimal Amount { get; set; }
    }
}
