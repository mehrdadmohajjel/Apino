using Apino.Domain.Entities;
using Apino.Domain.Enums;
using Apino.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = long.Parse(
                _http.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );
            var branchTitle = await _db.BranchUsers
                .Where(x =>
                    x.UserId == userId &&
                    x.Role == UserRole.BranchAdmin &&
                    x.IsActive
                )
                .Select(x => x.Branch.Title)
                .FirstOrDefaultAsync();

            return View("Default", branchTitle);
        }
    }

}
