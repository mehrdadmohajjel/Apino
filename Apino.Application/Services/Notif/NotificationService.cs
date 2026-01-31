using Apino.Application.Dtos.Notification;
using Apino.Application.Interfaces;
using Apino.Domain.Entities;
using Apino.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Services.Notif
{
    public class NotificationService : INotificationService
    {
        private readonly IReadDbContext _db;


        public NotificationService(IReadDbContext db)
        {
            _db = db;
        }

        public async Task CreateAsync(
            long userId,
            string title,
            string message,
            NotificationType type,
            long? branchId = null)
        {
            _db.Notifications.Add(new Notification
            {
                UserId = userId,
                BranchId = branchId,
                Title = title,
                Message = message,
                Type = type,
                IsRead = false,
                CreationDateTime = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();
        }

        public IQueryable<Notification> GetUserNotifications(long userId)
        {
            return _db.Notifications
                .Where(x => x.UserId == userId);
        }


        public async Task<List<Notification>> GetBranchNotifications(long branchId)
            => await _db.Notifications
                .Where(x => x.BranchId == branchId)
                .OrderByDescending(x => x.CreationDateTime)
                .ToListAsync();

        public async Task<List<Notification>> GetAllAsync()
            => await _db.Notifications
                .OrderByDescending(x => x.CreationDateTime)
                .ToListAsync();

        public async Task<int> GetUnreadCountAsync(long userId)
        {
            return await _db.Notifications
                .CountAsync(x => x.UserId == userId && !x.IsRead);
        }

        public async Task<List<Notification>> GetLastUnreadAsync(long userId, int take = 5)
        {
            return await _db.Notifications
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreationDateTime)
                .Take(take)
                .ToListAsync();
        }

        public async Task<List<Notification>> GetAllAsync(long userId)
        {
            return await _db.Notifications
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreationDateTime)
                .ToListAsync();
        }

        public async Task MarkAllAsReadAsync(long userId)
        {
            var items = await _db.Notifications
                .Where(x => x.UserId == userId && !x.IsRead)
                .ToListAsync();

            foreach (var n in items)
                n.IsRead = true;

            await _db.SaveChangesAsync();
        }
        public async Task<NotificationListViewModel> GetPagedAsync(
     long userId,
     int page,
     int pageSize,
     string search,
     bool? isRead)
        {
            var query = _db.Notifications
                .Where(n => n.UserId == userId);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(n =>
                    n.Title.Contains(search) ||
                    n.Message.Contains(search));

            if (isRead.HasValue)
                query = query.Where(n => n.IsRead == isRead.Value);

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(n => n.CreationDateTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(n => new NotificationItemVm
                {
                    Id = n.Id,
                    Title = n.Title,
                    Message = n.Message,
                    IsRead = n.IsRead,
                    CreatedAt = n.CreationDateTime
                })
                .ToListAsync();

            return new NotificationListViewModel
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = total,
                Search = search,
                IsRead = isRead
            };
        }


    }

}
