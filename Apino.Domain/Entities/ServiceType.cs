using Apino.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Domain.Entities
{
    public class ServiceType : BaseEntity
    {
        public string ServiceTypeTitle { get; set; }
        public string ServiceTypeEnglishName { get; set; }
        public bool IsSessionBased { get; set; }
    }
}
