using Apino.Application.Services.Notif;
using Apino.Domain.Entities;
using Apino.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Apino.Web.ViewComponents
{
    public class TopMenuViewComponent : ViewComponent
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _httpContext;
        private readonly INotificationService _notificationService;


        public TopMenuViewComponent(AppDbContext db, IHttpContextAccessor httpContext, INotificationService notificationService)
        {
            _db = db;
            _httpContext = httpContext;
            _notificationService = notificationService;
        }

        //public IViewComponentResult Invoke()
        //{
        //    var user = _httpContext.HttpContext.User;
        //    string username = user.Identity.IsAuthenticated ? user.Identity.Name : null;
        //    string role = null;

        //    if (user.Identity.IsAuthenticated)
        //    {
        //        if (user.IsInRole("SysAdmin")) role = "SysAdmin";
        //        else if (user.IsInRole("BranchAdmin")) role = "BranchAdmin";
        //        else role = "User";
        //    }

        //    // دریافت لیست شعب
        //    var branches = _db.Branches.OrderBy(b => b.Id).ToList();

        //    var model = new TopMenuViewModel
        //    {
        //        Username = username,
        //        Role = role,
        //        Branches = branches
        //    };

        //    return View(model);
        //}

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = _httpContext.HttpContext.User;

            string username = null;
            long? userId = null;

            if (user.Identity.IsAuthenticated)
            {
                username = user.Identity.Name;
                userId = long.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
            }

            var branches = await _db.Branches.OrderBy(b => b.Id).ToListAsync();

            var model = new TopMenuViewModel
            {
                Username = username,
                Branches = branches,
                UnreadNotifications = userId.HasValue
                    ? await _notificationService.GetUnreadCountAsync(userId.Value)
                    : 0,
                LastNotifications = userId.HasValue
                    ? await _notificationService.GetLastUnreadAsync(userId.Value)
                    : new List<Notification>()
            };

            return View(model);
        }
    }
}

    public class TopMenuViewModel
    {
        public string Username { get; set; }
        public string Role { get; set; }
        public List<Branch> Branches { get; set; }
    public int UnreadNotifications { get; set; }
    public List<Notification> LastNotifications { get; set; }

}

