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

        public async Task<List<Notification>> GetUserNotifications(long userId)
            => await _db.Notifications
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreationDateTime)
                .ToListAsync();

        public async Task<List<Notification>> GetBranchNotifications(long branchId)
            => await _db.Notifications
                .Where(x => x.BranchId == branchId)
                .OrderByDescending(x => x.CreationDateTime)
                .ToListAsync();

        public async Task<List<Notification>> GetAllAsync()
            => await _db.Notifications
                .OrderByDescending(x => x.CreationDateTime)
                .ToListAsync();
    }

}
