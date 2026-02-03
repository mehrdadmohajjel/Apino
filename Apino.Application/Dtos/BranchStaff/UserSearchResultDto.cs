using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Dtos.BranchStaff
{
    public class UserSearchResultDto
    {
        public long UserId { get; set; }
        public string DisplayName { get; set; } // ترکیب نام و موبایل
        public string Avatar { get; set; }
    }
}
