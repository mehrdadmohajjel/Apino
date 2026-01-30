using Apino.Domain.Entities;
using Apino.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Infrastructure.Data.Seeds
{
    public static class BranchStaffSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // نمونه کارمندان شعبه
            modelBuilder.Entity<BranchStaff>().HasData(
                new BranchStaff
                {
                    Id = 1,
                    UserId = 2,        // BranchAdmin که در UserSeed ایجاد شد
                    BranchId = 1,      // فرض می‌کنیم شعبه 1 داریم
                    Role = StaffRole.Manager
                },
                new BranchStaff
                {
                    Id = 2,
                    UserId = 3,        // یک کارمند دیگر (باید در UserSeed ایجاد شود)
                    BranchId = 1,
                    Role = StaffRole.Cashier
                },
                new BranchStaff
                {
                    Id = 3,
                    UserId = 4,        // یک کارمند دیگر (باید در UserSeed ایجاد شود)
                    BranchId = 1,
                    Role = StaffRole.Operator
                }
            );
        }
    }

}
