using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Dtos.ProductCategoryDto
{
    public class SaveCategoryVm
    {
        public long Id { get; set; }
        public string CategoryTitle { get; set; }
        public bool PayAtPlace { get; set; }
        public bool IsActive { get; set; }
        public IFormFile? Icon { get; set; }

    }

}
