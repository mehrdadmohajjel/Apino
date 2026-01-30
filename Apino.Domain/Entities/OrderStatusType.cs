using Apino.Domain.Base;
using Apino.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Domain.Entities
{
    public class OrderStatusType : BaseEntity
    {
        public string ShowName { get; set; } = null!;
        public string EnglishName { get; set; } = null!;
    }

}
