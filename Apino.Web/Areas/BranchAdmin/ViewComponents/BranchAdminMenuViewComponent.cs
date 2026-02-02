using Apino.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Apino.Web.Areas.BranchAdmin.ViewComponents
{
    public class BranchAdminMenuViewComponent : ViewComponent
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _http;

        public BranchAdminMenuViewComponent(AppDbContext db, IHttpContextAccessor http)
        {
            _db = db;
            _http = http;
        }

        public IViewComponentResult Invoke()
        {
            var userId = long.Parse(
                _http.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var branchTitle = _db.Users
                .Where(x => x.Id == userId)
                .Select(x => x.Branch.Title)
                .FirstOrDefault();

            return View("Default", branchTitle);
        }
    }

}
