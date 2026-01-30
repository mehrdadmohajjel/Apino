using Apino.Domain.Entities;
using Apino.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace Apino.Web.ViewComponents
{
    public class TopMenuViewComponent : ViewComponent
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _httpContext;

        public TopMenuViewComponent(AppDbContext db, IHttpContextAccessor httpContext)
        {
            _db = db;
            _httpContext = httpContext;
        }

        public IViewComponentResult Invoke()
        {
            var user = _httpContext.HttpContext.User;
            string username = user.Identity.IsAuthenticated ? user.Identity.Name : null;
            string role = null;

            if (user.Identity.IsAuthenticated)
            {
                if (user.IsInRole("SysAdmin")) role = "SysAdmin";
                else if (user.IsInRole("BranchAdmin")) role = "BranchAdmin";
                else role = "User";
            }

            // دریافت لیست شعب
            var branches = _db.Branches.OrderBy(b => b.Id).ToList();

            var model = new TopMenuViewModel
            {
                Username = username,
                Role = role,
                Branches = branches
            };

            return View(model);
        }
    }

    public class TopMenuViewModel
    {
        public string Username { get; set; }
        public string Role { get; set; }
        public List<Branch> Branches { get; set; }
    }
}

