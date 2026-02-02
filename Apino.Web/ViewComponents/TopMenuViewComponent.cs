using Apino.Application.Dtos;
using Apino.Application.Dtos.Notification;
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

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = _httpContext.HttpContext.User;

            long? userId = null;
            string role = null;

            if (user.Identity.IsAuthenticated)
            {
                userId = long.Parse(
                    user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? user.FindFirst("sub")!.Value
                );

                role = user.FindFirst(ClaimTypes.Role)?.Value;
            }

            string panelUrl = null;
            string panelTitle = null;

            if (role == "SystemAdmin")
            {
                panelUrl = "/SysAdmin";
                panelTitle = "ورود به پنل مدیر سیستم";
            }
            else if (role == "BranchAdmin")
            {
                panelUrl = "/BranchAdmin";
                panelTitle = "ورود به پنل مدیریت شعبه";
            }
            else if (role == "Staff")
            {
                panelUrl = "/Staff";
                panelTitle = "ورود به پنل کارمند";
            }

            var branches = _db.Branches.OrderBy(x => x.Id).ToList();

            var lastNotifications = new List<NotificationItemVm>();
            var unreadCount = 0;

            if (userId != null)
            {
                lastNotifications = await _db.Notifications
                    .Where(x => x.UserId == userId)
                    .OrderByDescending(x => x.CreationDateTime)
                    .Take(5)
                    .Select(n => new NotificationItemVm
                    {
                        Id = n.Id,
                        Title = n.Title,
                        Message = n.Message,
                        IsRead = n.IsRead,
                        CreatedAt = n.CreationDateTime
                    })
                    .ToListAsync();

                unreadCount = await _db.Notifications
                    .CountAsync(x => x.UserId == userId && !x.IsRead);
            }

            var model = new TopMenuViewModel
            {
                Username = user.Identity.IsAuthenticated ? user.Identity.Name : null,
                Role = role,
                PanelTitle = panelTitle,
                PanelUrl = panelUrl,
                Branches = branches,
                LastNotifications = lastNotifications,
                UnreadNotifications = unreadCount
            };

            return View(model);
        }

    }
}

   

