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
                role = user.FindFirst(ClaimTypes.Role)?.Value;
            }

            string panelUrl = null;
            string panelTitle = null;

            switch (role)
            {
                case "SystemAdmin":
                    panelUrl = "/SysAdmin";
                    panelTitle = "  پنل مدیر سیستم";
                    break;

                case "BranchAdmin":
                    panelUrl = "/BranchAdmin";
                    panelTitle = " پنل مدیریت شعبه";
                    break;

                case "Staff":
                    panelUrl = "/Staff";
                    panelTitle = " پنل کارمند";
                    break;
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

   

