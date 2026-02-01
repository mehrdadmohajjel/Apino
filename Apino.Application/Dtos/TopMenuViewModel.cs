using Apino.Application.Common.Helper;
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
        public List<Branch> Branches { get; set; }
        public int UnreadNotifications { get; set; }
        public List<Domain.Entities.Notification> LastNotifications { get; set; } = new();

    }
}
