using Apino.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace Apino.Web.Controllers
{
    public class BranchesController : Controller
    {
        private readonly AppDbContext _db;

        public BranchesController(AppDbContext db)
        {
            _db = db;
        }

        // صفحه اصلی
        public IActionResult Index()
        {
            var branches = _db.Branches.ToList();
            return View(branches); // شعبه‌ها برای نمایش در Featured Services
        }

        // اکشن برای دریافت دسته‌بندی‌ها به صورت JSON
        public IActionResult GetCategories(long branchId)
        {
            var categories = _db.ProductCategories
                                .Where(c => c.BranchId == branchId)
                                .Select(c => new
                                {
                                    c.Id,
                                    c.CategoryTitle,
                                    c.Slug,
                                    c.IconName
                                })
                                .ToList();

            return Json(categories);
        }

        public IActionResult GetProducts(long categoryId)
        {
            var products = _db.Products
                              .Where(p => p.ProductCategoryId == categoryId)
                              .Select(p => new
                              {
                                  p.Id,
                                  p.Title,
                                  p.Description,
                                  p.ImageName,
                                  p.Price
                              }).ToList();

            return Json(products);
        }
    }
}
