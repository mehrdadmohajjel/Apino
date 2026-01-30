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

        Task<List<Notification>> GetUserNotifications(long userId);

        Task<List<Notification>> GetBranchNotifications(long branchId);

        Task<List<Notification>> GetAllAsync();
    }
}
