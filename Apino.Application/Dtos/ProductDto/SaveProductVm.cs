using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Dtos.ProductDto
{
    public class SaveProductVm
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "انتخاب دسته‌بندی الزامی است")]
        public long ProductCategoryId { get; set; }

        [Required(ErrorMessage = "انتخاب نوع خدمت الزامی است")]
        public long ServiceTypeId { get; set; }

        [Required(ErrorMessage = "عنوان محصول الزامی است")]
        public string Title { get; set; }

        [Required(ErrorMessage = "قیمت الزامی است")]
        public decimal Price { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "موجودی الزامی است")]
        public int Stock { get; set; }

        public bool IsActive { get; set; }

        // فایل عکس (اختیاری در حالت ویرایش)
        public IFormFile? Image { get; set; }
    }
}
