using Apino.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Domain.Entities
{
    public class UserToken : BaseEntity
    {
        public long UserId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpireAt { get; set; }
        public DateTime CreationDateTime { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime? RevokedAt { get; set; }

        public User User { get; set; }
    }
}
