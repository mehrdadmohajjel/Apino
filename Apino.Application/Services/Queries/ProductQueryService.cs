using Apino.Application.Interfaces;
using Apino.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Apino.Application.Services.Queries
{
    public class ProductQueryService
    {
        private readonly IReadDbContext _context;

        public ProductQueryService(IReadDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetByCategoryAsync(long categoryId)
        {
            return await _context.Products
                .Where(x => x.ProductCategoryId == categoryId && x.IsActive)
                .OrderBy(x => x.Title)
                .ToListAsync();
        }
    }

}
