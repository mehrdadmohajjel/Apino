using Apino.Domain.Base;
using Apino.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Domain.Entities
{
    public class BranchUser : BaseEntity
    {
        public long UserId { get; set; }
        public long BranchId { get; set; }

        public UserRole Role { get; set; }   // SystemAdmin / BranchAdmin / Staff

        public DateTime StartWorkDate { get; set; }
        public DateTime? FinishWorkDate { get; set; } 

        public bool IsActive { get; set; }

        // Navigation
        public User User { get; set; }
        public Branch Branch { get; set; }
    }

}
