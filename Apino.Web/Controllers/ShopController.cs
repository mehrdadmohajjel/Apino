using Apino.Application.Dtos.Shop;
using Apino.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Apino.Web.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;
        public ShopController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(long branchId, long? categoryId, int page = 1)
        {
            const int pageSize = 9;

            // دسته‌بندی‌های منوی بالا
            var categories = await _context.ProductCategories
                .Where(x => x.BranchId == branchId && x.IsActive)
                .Select(x => new CategoryMenuViewModel
                {
                    Id = x.Id,
                    Title = x.CategoryTitle
                })
                .ToListAsync();

            // محصولات
            var query = _context.Products
                .Where(x => x.BranchId == branchId && x.IsActive);

            if (categoryId.HasValue)
                query = query.Where(x => x.ProductCategoryId == categoryId);

            var totalCount = await query.CountAsync();

            var products = await query
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ProductItemViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Price = x.Price,
                    ImageUrl = x.ImageName,
                    PayAtPlace =x.Category.PayAtPlace,
                    Stock = x.Stock
                })
                .ToListAsync();

            var model = new ShopPageViewModel
            {
                BranchId = branchId,
                CategoryId = categoryId,
                Categories = categories,
                Products = products,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };

            return View(model);
        }

        public async Task<IActionResult> ProductsPartial(
    long branchId,
    long? categoryId,
    int page = 1)
        {
            const int pageSize = 9;

            var query = _context.Products
                .Where(x => x.BranchId == branchId && x.IsActive);

            if (categoryId.HasValue)
                query = query.Where(x => x.ProductCategoryId == categoryId);

            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ProductItemViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Price = x.Price,
                    ImageUrl = x.ImageName,
                    PayAtPlace = x.Category.PayAtPlace,
                    Stock = x.Stock

                })
                .ToListAsync();

            return PartialView("_ProductList", products);
        }

    }

}
