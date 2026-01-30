using Apino.Application.Dtos.Cart;
using Apino.Application.Dtos.Order;
using Apino.Application.Interfaces;
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

        public OrderService(IReadDbContext db, ICartService cartService, IToolsService tools, IConfiguration config)
        {
            _db = db;
            _cartService = cartService;
            _tools = tools;
            _config = config;
        }

        public async Task<long> CreateFromCartAsync(long userId, long branchId)
        {
            var cart = await _cartService.GetActiveCartAsync(userId, branchId);

            if (cart == null || !cart.Items.Any())
                throw new Exception("سبد خرید خالی است");

            var order = new Domain.Entities.Order
            {
                UserId = userId,
                BranchId = branchId,
                OrderNumber = _tools.GenerateOrderNumber(),
                CreationDate = DateTime.UtcNow,
                CurrentStatusTypeId = (long)PaymentStatus.Pending,
                OrderDetails = cart.Items.Select(i => new OrderDetail
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

            var branchAdmin = await _db.Users
                .FirstOrDefaultAsync(a =>
                    a.BranchId == order.BranchId &&
                    a.Role == UserRole.BranchAdmin);

            var sysAdmin = await _db.Users
                .FirstOrDefaultAsync(a =>
                    a.Role == UserRole.SystemAdmin); // 🔴 BranchId حذف شد

            return new OrderUserMobileList
            {
                UserMobile = order.User?.Mobile,
                BranchAdminMobile = branchAdmin?.Mobile,
                SysAdmimMobile = sysAdmin?.Mobile,
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
                    .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new Exception("سفارش یافت نشد");

            var taxPercent = _config.GetValue<int>("Tax:Percent");

            var subTotal = order.OrderDetails.Sum(x => x.TotalPrice);
            var taxAmount = Math.Round(subTotal * taxPercent / 100, 0);
            var finalAmount = subTotal + taxAmount;

            return new OrderPaymentDto
            {
                OrderId = order.Id,
                TrackingNumber = long.Parse(order.OrderNumber),
                TotalAmount = finalAmount
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
            var order = await _db.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) return;

            foreach (var detail in order.OrderDetails)
            {
                detail.Product.Stock -= detail.Quantity;
                if (detail.Product.Stock < 0)
                    detail.Product.Stock = 0;
            }

            await _db.SaveChangesAsync();
        }

    }

}
