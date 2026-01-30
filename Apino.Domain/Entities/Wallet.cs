using Apino.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Domain.Entities
{
    public class Wallet : BaseEntity
    {
        public long UserId { get; set; }
        public decimal Balance { get; set; }
        public User User { get; set; }
        public ICollection<WalletTransaction> Transactions { get; set; }
    }

}
