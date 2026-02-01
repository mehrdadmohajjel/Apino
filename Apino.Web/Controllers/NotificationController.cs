using Apino.Application.Dtos.Notification;
using Apino.Application.Services.Notif;
using Apino.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading;

namespace Apino.Web.Controllers
{

    [Authorize]
    [Route("notifications")]
    public class NotificationController : Controller
    {
        private readonly INotificationService _service;

        public NotificationController(INotificationService service)
        {
            _service = service;
        }

        [HttpGet("count")]
        public async Task<IActionResult> Count()
        {
            var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var count = await _service.GetUnreadCountAsync(userId);
            return Json(new { count });
        }

        [HttpGet()]
        public async Task<IActionResult> Latest()
        {
            var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var items = await _service.GetLastUnreadAsync(userId);
            return PartialView("_NotificationDropdown", items);
        }
        [HttpGet("/notifications")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> List(
        bool? isRead,
        string search,
        int page = 1)
        {
            var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var model = await _service.GetPagedAsync(
                userId: userId,
                isRead: isRead,
                search: search,
                page: page,
                pageSize: 10
            );

            return PartialView("_NotificationTable", model);
        }

        // POST: /notification/mark-read
        [HttpPost("mark-read")]
        public async Task<IActionResult> MarkRead([FromBody] long notificationId)
        {
            var userId = long.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")!.Value
            );

            await _service.MarkAsReadAsync(notificationId, userId);

            var unreadCount = await _service.GetUnreadCountAsync(userId);

            return Ok(new
            {
                success = true,
                unreadCount
            });
        }
        //    public async Task<IActionResult> Index(
        //int page = 1,
        //bool? isRead = null,
        //string search = null)
        //    {
        //        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        //        const int pageSize = 10;

        //        IQueryable<Notification> query =
        //            _service.GetUserNotifications(userId);

        //        // فیلتر خوانده / نخوانده
        //        if (isRead.HasValue)
        //            query = query.Where(x => x.IsRead == isRead.Value);

        //        // سرچ
        //        if (!string.IsNullOrWhiteSpace(search))
        //            query = query.Where(x =>
        //                x.Title.Contains(search) ||
        //                x.Message.Contains(search));

        //        var total = await query.CountAsync();

        //        var items = await query
        //            .OrderByDescending(x => x.CreationDateTime)
        //            .Skip((page - 1) * pageSize)
        //            .Take(pageSize)
        //            .Select(x => new NotificationItemVm
        //            {
        //                Id = x.Id,
        //                Title = x.Title,
        //                Message = x.Message,
        //                IsRead = x.IsRead,
        //                CreatedAt = x.CreationDateTime
        //            })
        //            .ToListAsync();

        //        var model = new NotificationListViewModel
        //        {
        //            Items = items,
        //            Page = page,
        //            PageSize = pageSize,
        //            TotalCount = total,
        //            IsRead = isRead,
        //            Search = search
        //        };

        //        return View(model);
        //    }

        [HttpPost("mark-all-read")]
        public async Task<IActionResult> MarkAllRead()
        {
            var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _service.MarkAllAsReadAsync(userId);
            return Ok();
        }
    }

}
