using Apino.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Dtos.BranchStaff
{
    public class AddStaffVm
    {
        public long UserId { get; set; } // کاربری که انتخاب شده
        public UserRole Role { get; set; }
        public DateTime StartWorkDate { get; set; }
    }
}
