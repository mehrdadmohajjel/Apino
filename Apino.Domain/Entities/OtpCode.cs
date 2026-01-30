using Apino.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Domain.Entities
{
    public class OtpCode:BaseEntity
    {
        public string Mobile { get; set; } = null!;
        public string Code { get; set; } = null!;
        public DateTime ExpireAt { get; set; }
        public bool IsUsed { get; set; }
        public DateTime CreationDateTime { get; set; }
    }
}
