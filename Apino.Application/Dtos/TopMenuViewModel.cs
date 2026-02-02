using Apino.Application.Common.Helper;
using Apino.Application.Dtos.Notification;
using Apino.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Dtos
{
    public class TopMenuViewModel
    {
        public string Username { get; set; }
        public string Role { get; set; }
        public string PanelUrl { get; set; }
        public string PanelTitle { get; set; }

        public List<Branch> Branches { get; set; }
        public int UnreadNotifications { get; set; }
        public List<NotificationItemVm> LastNotifications { get; set; } = new();

    }
}
