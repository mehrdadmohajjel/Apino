using Apino.Application.Common;
using Apino.Application.Dtos.ProductDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Services.Products
{
    public interface IProductService
    {
        //Task<PagedResult<ProductListDto>> GetByBranchAsync(long branchId, int page, int pageSize = 10);
        Task<PagedResult<ProductListDto>> GetByBranchAsync(long branchId, int page, int pageSize, long? categoryId = null, long? serviceTypeId = null);

        Task<object> GetDropdownsAsync(long branchId); // برای گرفتن لیست دسته‌ها و نوع خدمات
        Task CreateAsync(long branchId, SaveProductVm model);
        Task UpdateAsync(long branchId, SaveProductVm model);
        Task ToggleActiveAsync(long id);
    }
}
