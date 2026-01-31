using Apino.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Domain.Entities
{
    public class Cart : BaseEntity
    {
        public long UserId { get; set; }
        public long BranchId { get; set; }
        public decimal Price { get; set; }

        // 🔴 اگر حتی یک آیتم Online-only باشد → true
        public bool OnlyOnlinePayment { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ICollection<CartItem> Items { get; set; }
    }

}
