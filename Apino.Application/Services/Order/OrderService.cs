using Apino.Application.Common.Helper;
using Apino.Application.Dtos.Cart;
using Apino.Application.Dtos.Order;
using Apino.Application.Interfaces;
using Apino.Application.Services.Auth;
using Apino.Application.Services.Cart;
using Apino.Domain.Entities;
using Apino.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly IReadDbContext _db;
        private readonly ICartService _cartService;
        private readonly IToolsService _tools;
        private readonly IConfiguration _config;
        private readonly ISmsSender _sms;

        public OrderService(IReadDbContext db, ICartService cartService, IToolsService tools, IConfiguration config,ISmsSender sms)
        {
            _db = db;
            _cartService = cartService;
            _tools = tools;
            _config = config;
            _sms = sms;
        }

        public async Task<long> CreateFromCartAsync(long userId, long branchId)
        {
            var cart = await _cartService.GetActiveCartAsync(userId, branchId);

            if (cart == null || !cart.OrderDetails.Any())
                throw new Exception("سبد خرید خالی است");

            var order = new Domain.Entities.Order
            {
                UserId = userId,
                BranchId = branchId,
                OrderNumber = _tools.GenerateOrderNumber(),
                CreationDate = DateTime.UtcNow,
                CurrentStatusTypeId = (long)PaymentStatus.Pending,
                OrderDetails = cart.OrderDetails.Select(i => new OrderDetail
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Product.Price,
                    TotalPrice = i.Quantity * i.Product.Price
                }).ToList()
            };

            _db.Orders.Add(order);

            _db.OrderStatuses.Add(new OrderStatus
            {
                Order = order,
                StatusTypeId = Convert.ToInt64(PaymentStatus.Pending),
                IsActive = true,
                CreationDateTime = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();

            await _cartService.ClearAsync(cart.Id);

            return order.Id;
        }


        public async Task<OrderUserMobileList> GetOrderMobileInfo(long orderId)
        {
            var order = await _db.Orders
                .Include(x => x.User)
                .Include(x => x.OrderDetails)
                    .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(x => x.Id == orderId);

            if (order == null)
                throw new Exception("Order not found");

            var branchAdmin = await _db.BranchUsers.Include(a=>a.User)
                .FirstOrDefaultAsync(a =>
                    a.BranchId == order.BranchId &&
                    a.Role == UserRole.BranchAdmin);

            var sysAdmin = await _db.BranchUsers.Include(a => a.User)
                .FirstOrDefaultAsync(a =>
                    a.BranchId == order.BranchId &&
                    a.Role == UserRole.SystemAdmin); // 🔴 BranchId حذف شد

            return new OrderUserMobileList
            {
                UserMobile = order.User?.Mobile,
                BranchAdminMobile = branchAdmin?.User.Mobile,
                SysAdmimMobile = sysAdmin?.User.Mobile,
                UserId = order.UserId,
                BranchAdminUserId=branchAdmin != null ? branchAdmin.Id : 0,
                SystemAdminUserId = sysAdmin != null ? sysAdmin.Id : 0, 
                OrderNumber = order.OrderNumber,
                TotalAmount = order.OrderDetails.Sum(d => d.TotalPrice),
                BranchId = order.BranchId,
                Items = order.OrderDetails.Select(d => new OrderItemDto
                {
                    Title = d.Product.Title,
                    Qty = d.Quantity
                }).ToList()
            };
        }

        // ===============================
        // Get Order For Payment
        // ===============================
        public async Task<OrderPaymentDto> GetOrderForPaymentAsync(long orderId)
        {
            var order = await _db.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o =>
                    o.Id == orderId &&
                    o.CurrentStatusTypeId == (long)PaymentStatus.Draft
                );

            if (order == null)
                throw new Exception("سفارش معتبر برای پرداخت یافت نشد");

            if (!order.OrderDetails.Any())
                throw new Exception("سفارش خالی است");

            var taxPercent = _config.GetValue<int>("Tax:Percent");

            var subTotal = order.OrderDetails.Sum(x => x.TotalPrice);
            var taxAmount = Math.Round(subTotal * taxPercent / 100, 0);
            var totalAmount = subTotal + taxAmount;

            return new OrderPaymentDto
            {
                OrderId = order.Id,
                UserId = order.UserId,
                BranchId = order.BranchId,
                SubTotal = subTotal,
                TaxAmount = taxAmount,
                TotalAmount = totalAmount,
                TrackingNumber = long.Parse(order.OrderNumber)
            };
        }

        // ===============================
        // Mark Order As Paid
        // ===============================
        public async Task MarkAsPaidAsync(long orderId, long paymentTypeId, string transactionCode)
        {
            var order = await _db.Orders.FirstOrDefaultAsync(x => x.Id == orderId);

            if (order == null)
                throw new Exception("سفارش یافت نشد");

            _db.OrderStatuses.Add(new OrderStatus
            {
                OrderId = order.Id,
                StatusTypeId = (long)PaymentStatus.Success,
                PaymentTypeId = paymentTypeId,
                IsActive = true,
                CreationDateTime = DateTime.Now
            });

            order.PaymentTransactionCode = transactionCode;

            await _db.SaveChangesAsync();
        }

        public async Task MarkAsPaidAsync(long orderId, string transactionCode)
        {
            // 🔐 گرفتن سفارش با جزئیات
            var order = await _db.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(d => d.Product)
                .Include(o => o.Statuses)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new Exception("سفارش یافت نشد");

            // ⛔ اگر قبلاً پرداخت شده، دوباره کاری نکن
            if (order.Statuses.Any(s => s.StatusTypeId == (long)PaymentStatus.Success))
                return;

            // 🔻 کاهش موجودی محصولات
            foreach (var item in order.OrderDetails)
            {
                if (item.Product.Stock < item.Quantity)
                    throw new Exception($"موجودی محصول «{item.Product.Title}» کافی نیست");

                item.Product.Stock -= item.Quantity;
            }

            // 🔄 وضعیت سفارش
            order.CurrentStatusTypeId = (long)PaymentStatus.Success;

            // 🧾 ثبت وضعیت پرداخت
            _db.OrderStatuses.Add(new OrderStatus
            {
                OrderId = order.Id,
                StatusTypeId = (long)PaymentStatus.Success,
                PaymentTypeId = (long)PaymentMethod.Online,
                IsActive = true,
                CreationDateTime = DateTime.UtcNow
            });

            // 💾 ذخیره کد تراکنش (اگر داری)
            order.PaymentTransactionCode = transactionCode;

            await _db.SaveChangesAsync();
        }


        public async Task DecreaseProductStockAsync(long orderId)
        {
            using var tx = await (_db as DbContext)!.Database.BeginTransactionAsync();
            var order = await _db.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new Exception("سفارش یافت نشد");

            foreach (var item in order.OrderDetails)
            {
                if (item.Product.Stock < item.Quantity)
                    throw new Exception($"موجودی محصول {item.Product.Title} کافی نیست");

                item.Product.Stock -= item.Quantity;
            }

            await _db.SaveChangesAsync();
            await tx.CommitAsync();
        }
        //=============================
        //سفارذشات شعبه
        //=============================
        public async Task<List<BranchOrderListDto>> GetOrdersAsync(long branchId)
        {
            // فقط سفارشاتی که پرداخت شده‌اند یا جلوتر هستند (StatusId >= 2)
            var orders = await _db.Orders
                .Include(o => o.User)
                .ThenInclude(u => u.UserProfile)
                .Include(o => o.Statuses)
                .ThenInclude(s => s.StatusType)
                .Where(o => o.BranchId == branchId && o.CurrentStatusTypeId >= 2)
                .OrderByDescending(o => o.Id)
                .ToListAsync();

            return orders.Select(o =>
            {
                var nextStatus = GetNextStatus(o.CurrentStatusTypeId);
                return new BranchOrderListDto
                {
                    Id = o.Id,
                    OrderNumber = o.OrderNumber,
                    CustomerName = o.User.UserProfile?.FullName ?? "کاربر مهمان",
                    CustomerMobile = o.User.Mobile,
                    TotalAmount = o.TotalAmount,
                    CreateDate = PersianDateHelper.ToShamsi(o.CreationDate),
                    StatusId = o.CurrentStatusTypeId,
                    StatusTitle = o.Statuses.FirstOrDefault(s => s.IsActive)?.StatusType?.ShowName ?? "-",

                    // تعیین دکمه بعدی
                    NextStatusId = nextStatus.Id,
                    NextStatusTitle = nextStatus.Title,
                    IsFinished = o.CurrentStatusTypeId == 8 || o.CurrentStatusTypeId == 6 // تحویل شده یا لغو شده
                };
            }).ToList();
        }

        public async Task ChangeStatusAsync(long orderId, long branchId, long userId)
        {
            var order = await _db.Orders
                .Include(o => o.User) // برای موبایل مشتری
                .Include(o => o.Statuses)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.BranchId == branchId);

            if (order == null) throw new Exception("سفارش یافت نشد.");

            // پیدا کردن BranchUserId کسی که لاگین کرده
            var branchUser = await _db.BranchUsers
                .FirstOrDefaultAsync(bu => bu.BranchId == branchId && bu.UserId == userId);

            if (branchUser == null) throw new Exception("شما دسترسی لازم در این شعبه را ندارید.");

            // تعیین وضعیت بعدی بر اساس وضعیت فعلی
            var nextStatusInfo = GetNextStatus(order.CurrentStatusTypeId);
            if (nextStatusInfo.Id == 0) throw new Exception("تغییر وضعیت برای این مرحله امکان‌پذیر نیست.");

            long newStatusId = nextStatusInfo.Id;

            // 1. غیرفعال کردن وضعیت قبلی
            var oldStatus = order.Statuses.FirstOrDefault(s => s.IsActive);
            if (oldStatus != null) oldStatus.IsActive = false;

            // 2. افزودن وضعیت جدید
            var newStatus = new OrderStatus
            {
                OrderId = order.Id,
                StatusTypeId = newStatusId,
                BranchUserId = branchUser.Id, // ذخیره شناسه کارمند
                IsActive = true,
                CreationDateTime = DateTime.Now
            };
            _db.OrderStatuses.Add(newStatus);

            // 3. آپدیت خود سفارش
            order.CurrentStatusTypeId = newStatusId;

            await _db.SaveChangesAsync();

            // 4. ارسال پیامک (غیرهمزمان برای جلوگیری از کندی)
            _ = SendNotificationsAsync(order, branchId, newStatusId);
        }

        // منطق توالی وضعیت‌ها
        private (long Id, string Title) GetNextStatus(long currentId)
        {
            return currentId switch
            {
                2 => (3, "قبول سفارش"),         // Paid -> Accepted
                3 => (4, "شروع آماده‌سازی"),   // Accepted -> Preparing
                4 => (5, "آماده تحویل"),       // Preparing -> Done
                5 => (8, "تحویل به مشتری"),    // Done -> Delivered
                _ => (0, "")                   // پایان یا نامعتبر
            };
        }

        // مدیریت ارسال پیامک‌ها
        private async Task SendNotificationsAsync(Domain.Entities.Order order, long branchId, long statusId)
        {
            try
            {
                // متن پیامک بر اساس وضعیت
                string message = statusId switch
                {
                    3 => $"سفارش {order.OrderNumber} توسط شعبه تایید شد و در نوبت قرار گرفت.",
                    4 => $"سفارش {order.OrderNumber} در حال آماده‌سازی است.",
                    5 => $"سفارش {order.OrderNumber} آماده تحویل است. لطفاً مراجعه فرمایید.",
                    8 => $"سفارش {order.OrderNumber} تحویل داده شد. از خرید شما سپاسگزاریم.",
                    _ => ""
                };

                if (string.IsNullOrEmpty(message)) return;

                // 1. ارسال به مشتری
                await _sms.SendAsync(order.User.Mobile, message);

                // 2. ارسال به مدیر شعبه (BranchAdmin)
                var branchAdminMobile = await _db.BranchUsers
                    .Include(bu => bu.User)
                    .Where(bu => bu.BranchId == branchId && bu.Role == UserRole.BranchAdmin && bu.IsActive)
                    .Select(bu => bu.User.Mobile)
                    .FirstOrDefaultAsync();

                if (!string.IsNullOrEmpty(branchAdminMobile))
                {
                    string adminMsg = $"مدیر گرامی، وضعیت سفارش {order.OrderNumber} به '{GetStatusName(statusId)}' تغییر یافت.";
                    await _sms.SendAsync(branchAdminMobile, adminMsg);
                }
            }
            catch (Exception ex)
            {
                // لاگ کردن خطا - نباید پروسه اصلی را متوقف کند
                Console.WriteLine($"SMS Error: {ex.Message}");
            }
        }

        private string GetStatusName(long id)
        {
            return id switch { 3 => "تایید شده", 4 => "در حال آماده‌سازی", 5 => "آماده تحویل", 8 => "تحویل شده", _ => "" };
        }


    }

}
