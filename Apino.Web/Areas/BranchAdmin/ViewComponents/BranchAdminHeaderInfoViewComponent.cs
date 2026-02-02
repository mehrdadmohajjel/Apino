using Apino.Domain.Enums;
using Apino.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Apino.Web.Areas.BranchAdmin.ViewComponents
{
    public class BranchAdminHeaderInfoViewComponent : ViewComponent
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _http;

        public BranchAdminHeaderInfoViewComponent(AppDbContext db, IHttpContextAccessor http)
        {
            _db = db;
            _http = http;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("Default");
        }
    }

}
