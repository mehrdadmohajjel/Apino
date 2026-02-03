using Apino.Application.Dtos.BranchStaff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Services.BranchStaff
{
    public interface IBranchStaffService
    {
        Task<List<BranchStaffListDto>> GetStaffByBranchAsync(long branchId);
        Task<List<UserSearchResultDto>> SearchUsersAsync(string term);
        Task AddStaffAsync(long branchId, AddStaffVm vm);
        Task ToggleActiveAsync(long branchUserId);
        Task RemoveStaffAsync(long branchUserId);
    }
}
