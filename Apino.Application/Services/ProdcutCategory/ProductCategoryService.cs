using Apino.Application.Common.Helper;
using Apino.Application.Interfaces;
using Apino.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Services.ProdcutCategory
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IReadDbContext _db;
        private readonly IWebHostEnvironment _env;

        public ProductCategoryService(IReadDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<List<ProductCategory>> GetByBranchAsync(long branchId)
        {
            return await _db.ProductCategories
                .Where(x => x.BranchId == branchId)
                .OrderByDescending(x => x.CreationDateTime)
                .ToListAsync();
        }

 

        public async Task UpdateAsync(long id, string title, bool payAtPlace, bool isActive,IFormFile icon)
        {
            var category = await _db.ProductCategories.FindAsync(id);
            if (category == null) throw new Exception("دسته‌بندی یافت نشد");

            category.CategoryTitle = title;
            category.PayAtPlace = payAtPlace;
            category.Slug = SlugHelper.Generate(title, category.BranchId);
            category.IsActive =isActive;
            if (icon != null)
                category.IconName = await SaveIcon(icon);

            await _db.SaveChangesAsync();
        }

        public async Task ToggleActiveAsync(long id)
        {
            var category = await _db.ProductCategories.FindAsync(id);
            if (category == null) return;

            category.IsActive = !category.IsActive;
            await _db.SaveChangesAsync();
        }

        private async Task<string> SaveIcon(IFormFile file)
        {
            if (file == null) return null;

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var path = Path.Combine(_env.WebRootPath, "images/category", fileName);

            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            return fileName;
        }
        public async Task<long> CreateAsync(long branchId, string title, bool payAtPlace, IFormFile icon)
        {
            var slug = SlugHelper.Generate(title, branchId);

            var category = new ProductCategory
            {
                BranchId = branchId,
                CategoryTitle = title,
                Slug = slug,
                PayAtPlace = payAtPlace,
                IsActive = true,
                CreationDateTime = DateTime.UtcNow
            };

            // اصلاح منطقی: ذخیره آیکون در صورت وجود
            if (icon != null)
            {
                category.IconName = await SaveIcon(icon);
            }

            _db.ProductCategories.Add(category);

            // استفاده از await نیازمند async بودن متد است
            await _db.SaveChangesAsync();

            return category.Id;
        }

    }

}
