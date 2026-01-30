using Apino.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Domain.Entities
{
    public class CategoryAccess : BaseEntity
    {
        public long ProductCategoryId { get; set; }
        public long UserId { get; set; }
    }
}
