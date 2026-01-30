using Apino.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apino.Domain.Enums;

namespace Apino.Domain.Entities
{
    public class BranchStaff : BaseEntity
    {
        public long UserId { get; set; }
        public long BranchId { get; set; }
        public StaffRole Role { get; set; }

        public User User { get; set; } = null!;
        public Branch Branch { get; set; } = null!;
    }
}
