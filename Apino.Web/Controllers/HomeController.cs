using Apino.Application.Dtos;
using Apino.Infrastructure.Data;
using Apino.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Apino.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            // دریافت لیست شعب
            var branches = _db.Branches
                              .Select(b => new BranchViewModel
                              {
                                  Id = b.Id,
                                  Title = b.Title,
                                  Description = b.Description,
                                  ImageUrl = b.ImageUrl
                              })
                              .ToList();

            return View(branches); // ارسال به View
        }
    }  
}