using Apino.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Apino.Web.Areas.BranchAdmin.ViewComponents
{


    public class TopMenuViewComponent : ViewComponent
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _http;

        public TopMenuViewComponent(AppDbContext db, IHttpContextAccessor http)
        {
            _db = db;
            _http = http;
        }

        public IViewComponentResult Invoke()
        {
            var userId = long.Parse(
                _http.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );

            var user = _db.Users.First(x => x.Id == userId);

            var notifs = _db.Notifications
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreationDateTime)
                .Take(5)
                .ToList();

            return View(new
            {
                User = user,
                Notifications = notifs
            });
        }
    }

}
