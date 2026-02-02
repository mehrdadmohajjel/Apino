using Apino.Domain.Base;
using Apino.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Mobile { get; set; } = null!;
        public bool IsActive { get; set; }

        public UserRole Role { get; set; } // فقط برای مشتری‌ها (User)

        public DateTime CreationDatetime { get; set; }

        public UserProfile? UserProfile { get; set; }

        public List<BranchUser> BranchUsers { get; set; }
    }


}
