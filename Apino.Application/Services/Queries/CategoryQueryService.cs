using Apino.Application.Interfaces;
using Apino.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Services.Queries
{
    public class CategoryQueryService
    {
        private readonly IReadDbContext _context;

        public CategoryQueryService(IReadDbContext context)
        {
            _context = context;
        }


        public async Task<List<ProductCategory>> GetByBranchAsync(long branchId)
        {
            return await _context.ProductCategories
                .Where(x => x.BranchId == branchId && x.IsActive)
                .OrderBy(x => x.CategoryTitle)
                .ToListAsync();
        }
    }

}
