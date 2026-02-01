using Apino.Application.Dtos.Notification;
using Apino.Domain.Entities;
using Apino.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Services.Notif
{
    public interface INotificationService
    {
        Task CreateAsync(
            long userId,
            string title,
            string message,
            NotificationType type,
            long? branchId = null
        );

        IQueryable<Notification> GetUserNotifications(long userId);
        Task<List<Notification>> GetBranchNotifications(long branchId);

        Task<int> GetUnreadCountAsync(long userId);

        Task<List<Notification>> GetLastUnreadAsync(long userId, int take = 5);

        Task<List<Notification>> GetAllAsync(long userId);
        Task MarkAsReadAsync(long notificationId, long userId);

        Task MarkAllAsReadAsync(long userId);
        Task<NotificationListViewModel> GetPagedAsync(long userId, int page, int pageSize, string search,
     bool? isRead);

    }
}
