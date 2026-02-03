using Apino.Application.Common;
using Apino.Application.Dtos.ProductDto;
using Apino.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IReadDbContext _db;
        private readonly IWebHostEnvironment _env;

        public ProductService(IReadDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<PagedResult<ProductListDto>> GetByBranchAsync(long branchId, int page, int pageSize, long? categoryId = null, long? serviceTypeId = null)
        {
            var query = _db.Products
                .Include(x => x.Category)
                .Include(x => x.ServiceType)
                .Where(x => x.BranchId == branchId);

            // --- اعمال فیلترها ---
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(x => x.ProductCategoryId == categoryId.Value);
            }

            if (serviceTypeId.HasValue && serviceTypeId.Value > 0)
            {
                query = query.Where(x => x.ServiceTypeId == serviceTypeId.Value);
            }
            // -------------------

            // محاسبه تعداد بعد از فیلتر
            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ProductListDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Price = x.Price,
                    Stock = x.Stock,
                    IsActive = x.IsActive,
                    ImageName = x.ImageName,
                    Description = x.Description,
                    CategoryTitle = x.Category.CategoryTitle,
                    ProductCategoryId = x.ProductCategoryId,
                    ServiceTypeTitle = x.ServiceType.ServiceTypeTitle,
                    ServiceTypeId = x.ServiceTypeId
                }).ToListAsync();

            return new PagedResult<ProductListDto>
            {
                Items = items,
                TotalCount = totalCount,
                CurrentPage = page,
                PageSize = pageSize
            };
        }
        //public async Task<PagedResult<ProductListDto>> GetByBranchAsync(long branchId, int page, int pageSize = 10)
        //{
        //    var query = _db.Products
        //        .Include(x => x.Category)
        //        .Include(x => x.ServiceType)
        //        .Where(x => x.BranchId == branchId);

        //    // 1. شمارش کل رکوردها قبل از صفحه بندی
        //    var totalCount = await query.CountAsync();

        //    // 2. دریافت داده‌های صفحه جاری
        //    var items = await query
        //        .OrderByDescending(x => x.Id)
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize)
        //        .Select(x => new ProductListDto
        //        {
        //            Id = x.Id,
        //            Title = x.Title,
        //            Price = x.Price,
        //            Stock = x.Stock,
        //            IsActive = x.IsActive,
        //            ImageName = x.ImageName,
        //            Description = x.Description,
        //            CategoryTitle = x.Category.CategoryTitle,
        //            ProductCategoryId = x.ProductCategoryId,
        //            ServiceTypeTitle = x.ServiceType.ServiceTypeTitle,
        //            ServiceTypeId = x.ServiceTypeId
        //        }).ToListAsync();

        //    return new PagedResult<ProductListDto>
        //    {
        //        Items = items,
        //        TotalCount = totalCount,
        //        CurrentPage = page,
        //        PageSize = pageSize
        //    };
        //}

        public async Task<object> GetDropdownsAsync(long branchId)
        {
            var categories = await _db.ProductCategories
                .Where(x => x.BranchId == branchId && x.IsActive)
                .Select(x => new { x.Id, x.CategoryTitle })
                .ToListAsync();

            var serviceTypes = await _db.ServiceTypes
                .Select(x => new { x.Id, Title = x.ServiceTypeTitle }) // فرض بر این است که ServiceType سراسری است
                .ToListAsync();

            return new { categories, serviceTypes };
        }

        public async Task CreateAsync(long branchId, SaveProductVm model)
        {
            var product = new Domain.Entities.Product
            {
                BranchId = branchId,
                Title = model.Title,
                Price = model.Price,
                Stock = model.Stock,
                Description = model.Description ?? "",
                IsActive = true,
                ProductCategoryId = model.ProductCategoryId,
                ServiceTypeId = model.ServiceTypeId
            };

            if (model.Image != null)
                product.ImageName = await SaveImage(model.Image);
            else
                product.ImageName = ""; // یا یک عکس پیش‌فرض

            _db.Products.Add(product);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(long branchId, SaveProductVm model)
        {
            var product = await _db.Products.FirstOrDefaultAsync(x => x.Id == model.Id && x.BranchId == branchId);
            if (product == null) throw new Exception("محصول یافت نشد");

            product.Title = model.Title;
            product.Price = model.Price;
            product.Stock = model.Stock;
            product.Description = model.Description ?? "";
            product.ProductCategoryId = model.ProductCategoryId;
            product.ServiceTypeId = model.ServiceTypeId;
            product.IsActive = model.IsActive; // در ویرایش وضعیت هم میاد

            if (model.Image != null)
            {
                // اینجا می‌توان عکس قبلی را حذف کرد
                product.ImageName = await SaveImage(model.Image);
            }

            await _db.SaveChangesAsync();
        }

        public async Task ToggleActiveAsync(long id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product != null)
            {
                product.IsActive = !product.IsActive;
                await _db.SaveChangesAsync();
            }
        }

        private async Task<string> SaveImage(IFormFile file)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var folderPath = Path.Combine(_env.WebRootPath, "images/products");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var path = Path.Combine(folderPath, fileName);
            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            return fileName;
        }
    }
}