using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Apino.Infrastructure.Helper
{
    public static class SlugHelper
    {
        public static string Generate(string title, long branchId)
        {
            if (string.IsNullOrWhiteSpace(title))
                return branchId.ToString();

            var slug = title.Trim();

            // تبدیل فاصله و نیم‌فاصله به -
            slug = slug
                .Replace("‌", " ")   // نیم‌فاصله
                .Replace(" ", "-");

            // حذف کاراکترهای غیرمجاز
            slug = Regex.Replace(slug, @"[^آ-یa-zA-Z0-9\-]", "");

            // جلوگیری از --
            slug = Regex.Replace(slug, @"-+", "-");

            return $"{slug}-{branchId}";
        }
    }

}
