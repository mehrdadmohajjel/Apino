using Apino.Application.Common;
using Apino.Application.Common.Helper;
using Apino.Application.Dtos.Payment;
using Apino.Application.Interfaces;
using Apino.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Services.Payments
{
    public class PaymentReportService : IPaymentReportService
    {
        private readonly IReadDbContext _db;

        public PaymentReportService(IReadDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResult<PaymentReportItemDto>> GetBranchPaymentsAsync(long branchId, string fromDateShamsi, string toDateShamsi, int page = 1, int pageSize = 10)
        {
            // 1. استفاده از AsNoTracking برای سرعت بیشتر و جلوگیری از خطاهای رهگیری
            // نکته: اینجا Include ها را حذف کردیم چون در Select پایین، EF خودش Join ها را می سازد.
            var query = _db.Payments.AsNoTracking();

            // 2. اعمال فیلتر شعبه
            // فرض بر این است که p.Orders یک رابطه تکی است (نه لیست). 
            // اگر لیست است باید بنویسید: p.Orders.Any(o => o.BranchId == branchId)
            query = query.Where(p => p.Orders != null && p.Orders.BranchId == branchId);

            // 3. فیلتر تاریخ (استفاده از هلپری که در مرحله قبل ساختیم)
            if (!string.IsNullOrEmpty(fromDateShamsi))
            {
                var gFromDate = PersianDateHelper.ToGregorian(fromDateShamsi);
                if (gFromDate.HasValue)
                {
                    query = query.Where(p => p.CreatedAt >= gFromDate.Value);
                }
            }

            if (!string.IsNullOrEmpty(toDateShamsi))
            {
                var gToDate = PersianDateHelper.ToGregorian(toDateShamsi);
                if (gToDate.HasValue)
                {
                    // تنظیم زمان به آخرین لحظه روز (23:59:59)
                    var endOfDay = gToDate.Value.AddDays(1).AddTicks(-1);
                    query = query.Where(p => p.CreatedAt <= endOfDay);
                }
            }

            // 4. محاسبه تعداد کل (اینجا معمولا خطا رخ می‌داد چون کوئری قبلی سنگین بود)
            var totalCount = await query.CountAsync();

            // 5. دریافت داده‌ها و تبدیل به DTO
            var items = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PaymentReportItemDto
                {
                    Id = p.Id,
                    // دسترسی امن به فیلدها (Null Check)
                    OrderNumber = p.Orders != null ? p.Orders.OrderNumber : "-",

                    // چک کردن نال بودن پروفایل کاربر
                    CustomerName = p.Orders.User.UserProfile.FullName ?? p.Orders.User.Mobile ?? "ناشناس",

                    Amount = p.Amount,

                    // اگر متد GetMethodTitle سمت دیتابیس ترجمه نمی‌شود، باید داده را خام بگیرید و در مموری تبدیل کنید
                    // اما معمولا برای Enum مشکلی نیست
                    MethodTitle = p.Method.ToString(),
                    StatusTitle = p.Status.ToString(),
                    StatusId = (int)p.Status,

                    // تبدیل تاریخ باید سمت کلاینت یا بعد از واکشی انجام شود، اما متد ToShamsi ما
                    // قابلیت ترجمه به SQL را ندارد. EF Core اینجا خطا می‌دهد.
                    // راه حل: تاریخ میلادی را بگیرید و در مرحله بعد تبدیل کنید.
                    CreateDateRaw = p.CreatedAt
                })
                .ToListAsync();

            // 6. اصلاح نهایی داده‌ها (تبدیل تاریخ و Enum ها در حافظه)
            // این کار باعث می‌شود خطای "LINQ Expression could not be translated" نگیرید
            var finalItems = items.Select(x => {
                x.CreateDate = PersianDateHelper.ToShamsi(x.CreateDateRaw);
                x.CreateTime = x.CreateDateRaw.ToString("HH:mm");
                x.MethodTitle = GetMethodTitle(Enum.Parse<PaymentMethod>(x.MethodTitle)); // متد خودتان
                x.StatusTitle = GetStatusTitle(Enum.Parse<PaymentStatus>(x.StatusTitle)); // متد خودتان
                return x;
            }).ToList();

            return new PagedResult<PaymentReportItemDto>
            {
                Items = finalItems, // لیست نهایی
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        // هلپر تبدیل تاریخ (بهتر است در PersianDateHelper باشد اما اینجا برای تکمیل کد می‌آوریم)
        private DateTime ConvertShamsiToGregorian(string shamsiDate)
        {
            var parts = shamsiDate.Split('/');
            var pc = new PersianCalendar();
            return pc.ToDateTime(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]), 0, 0, 0, 0);
        }

        private static string GetMethodTitle(PaymentMethod method)
        {
            return method switch
            {
                PaymentMethod.Online => "پرداخت آنلاین",
                PaymentMethod.Wallet => "کیف پول",
                PaymentMethod.Cash => "پرداخت حضوری",
                _ => "نامشخص"
            };
        }

        private static string GetStatusTitle(PaymentStatus status)
        {
            return status switch
            {
                PaymentStatus.Success => "موفق",
                PaymentStatus.Pending => "در انتظار",
                PaymentStatus.PaymentFailed => "ناموفق",
                PaymentStatus.Canceled => "لغو شده",
                _ => status.ToString() // سایر وضعیت‌ها
            };
        }
    }
}