using Apino.Domain.Base;
using Apino.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public long UserId { get; set; }          // گیرنده اعلان
        public long? BranchId { get; set; }       // برای فیلتر مدیر شعبه

         public NotificationType Type { get; set; }

        public string Title { get; set; }
        public string Message { get; set; }

        public bool IsRead { get; set; }
        public DateTime CreationDateTime { get; set; }

        public User User { get; set; }
    }

}
