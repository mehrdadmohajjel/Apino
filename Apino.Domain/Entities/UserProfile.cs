using Apino.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Domain.Entities
{
    public class UserProfile : BaseEntity
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public long UserId { get; set; }
        public string? TelegramId { get; set; }
        public string? LinkedInAddress { get; set; }
        public string? InstagramUserName { get; set; }
        public bool IsProfileCompleted { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string? Avatar { get; set; }
    }
}
