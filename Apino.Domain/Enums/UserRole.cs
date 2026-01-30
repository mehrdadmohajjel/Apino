using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Domain.Enums
{
    public enum UserRole
    {
        SystemAdmin = 1,  // دسترسی به کل سیستم
        BranchAdmin = 2,  // مدیر شعبه
        Staff = 3,        // کارکنان
        User = 4          // مشتری
    }
}
