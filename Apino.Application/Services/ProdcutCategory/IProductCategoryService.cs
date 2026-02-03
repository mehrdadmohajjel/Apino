using Apino.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Services.ProdcutCategory
{
    public interface IProductCategoryService
    {
        Task<List<ProductCategory>> GetByBranchAsync(long branchId);
        Task<long> CreateAsync(long branchId, string title, bool payAtPlace, IFormFile icon);
        Task UpdateAsync(long id, string title, bool payAtPlace,bool isActive, IFormFile icon);
        Task ToggleActiveAsync(long id);
    }

}
