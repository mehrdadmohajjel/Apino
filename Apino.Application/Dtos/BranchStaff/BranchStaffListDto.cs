using Apino.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Dtos.BranchStaff
{
    public class BranchStaffListDto
    {
        public long Id { get; set; } // BranchUserId (نه UserId)
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string Avatar { get; set; }
        public UserRole Role { get; set; }
        public string RoleTitle => Role == UserRole.BranchAdmin ? "مدیر شعبه" : "کارمند";
        public string StartDate { get; set; } // شمسی شده
        public bool IsActive { get; set; }
    }
}
