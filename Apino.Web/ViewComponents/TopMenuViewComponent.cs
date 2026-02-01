using Apino.Application.Dtos;
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

            long? userId = null;
            if (user.Identity.IsAuthenticated)
            {
                userId = long.Parse(
                    user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? user.FindFirst("sub")!.Value
                );
            }

            var notifications = new List<Notification>();
            var unreadCount = 0;

            if (userId != null)
            {
                notifications = _db.Notifications
                    .Where(x => x.UserId == userId)
                    .OrderByDescending(x => x.CreationDateTime)
                    .Take(5)
                    .ToList();

                unreadCount = _db.Notifications
                    .Count(x => x.UserId == userId && !x.IsRead);
            }

            var model = new TopMenuViewModel
            {
                Username = user.Identity.IsAuthenticated ? user.Identity.Name : null,
                Role = user.Identity.IsAuthenticated
                    ? user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value
                    : null,
                Branches = _db.Branches.OrderBy(x => x.Id).ToList(),
                LastNotifications = notifications,
                UnreadNotifications = unreadCount
            };

            return View(model);
        }
    }
}

   

