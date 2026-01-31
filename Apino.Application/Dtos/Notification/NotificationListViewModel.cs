using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Dtos.Notification
{
    public class NotificationListViewModel
    {
        public List<NotificationItemVm> Items { get; set; } = new();

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public string Search { get; set; }
        public bool? IsRead { get; set; } // null = همه

        public int TotalPages =>
            (int)Math.Ceiling((double)TotalCount / PageSize);
    }


}
