using Apino.Application.Dtos.Payment;
using Apino.Application.Services.Auth;
using Apino.Application.Services.Notif;
using Apino.Application.Services.Order;
using Apino.Domain.Entities;
using Apino.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parbad;
using Parbad.AspNetCore;
using System.Security.Claims;

namespace Apino.Web.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ISmsSender _sms;
        private readonly INotificationService _notificationService;
        private readonly IOnlinePayment _onlinePayment;

        public PaymentController(
            IOrderService orderService,
            ISmsSender sms,
            INotificationService notificationService,
            IOnlinePayment onlinePayment)
        {
            _orderService = orderService;
            _sms = sms;
            _notificationService = notificationService;
            _onlinePayment = onlinePayment;
        }

        // =======================
        // شروع پرداخت
        // =======================
        [HttpGet]
        public async Task<IActionResult> Pay(long orderId)
        {
            var userId = long.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub")
            );

            var order = await _orderService.GetOrderForPaymentAsync(orderId);

            if (order == null || order.UserId != userId)
                return Unauthorized();

            if (order.TotalAmount <= 0)
                return View("PayRequestError");

            var callbackUrl = Url.Action(
                "Verify",
                "Payment",
                new { orderId },
                Request.Scheme);

            var result = await _onlinePayment.RequestAsync(invoice =>
            {
                invoice
                    .SetAmount(order.TotalAmount)
                    .SetGateway("Mellat")
                    .SetTrackingNumber(order.TrackingNumber)
                    .SetCallbackUrl(callbackUrl);
            });

            if (result.IsSucceed)
                return result.GatewayTransporter.TransportToGateway();

            return View("PayRequestError");
        }

        // =======================
        // برگشت از بانک
        // =======================
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Verify(long orderId)
        {
            var invoice = await _onlinePayment.FetchAsync();

            if (invoice == null)
                return View("PaymentFailed");

            // جلوگیری از دوبار Verify
            if (invoice.IsAlreadyVerified)
                return RedirectToAction("Success", new { trackingNumber = invoice.TrackingNumber });

            if (invoice.Status != PaymentFetchResultStatus.ReadyForVerifying)
                return View("PaymentFailed");

            var verify = await _onlinePayment.VerifyAsync(invoice);

            if (!verify.IsSucceed)
                return View("PaymentFailed");

            // ثبت پرداخت
            await _orderService.MarkAsPaidAsync(
                orderId,
                (long)PaymentMethod.Online,
                verify.TransactionCode
            );

            // کاهش موجودی
            await _orderService.DecreaseProductStockAsync(orderId);

            // اطلاعات پیام
            var data = await _orderService.GetOrderMobileInfo(orderId);

            var itemsText = string.Join("، ",
                data.Items.Select(i => $"{i.Title}×{i.Qty}")
            );

            // SMS کاربر
            await _sms.SendAsync(
                data.UserMobile,
                $"پرداخت شما با موفقیت انجام شد ✅\n" +
                $"شماره سفارش: {data.OrderNumber}\n" +
                $"کد رهگیری: {data.TrackingNumber}\n" +
                $"مبلغ: {data.TotalAmount:N0} تومان\n" +
                $"اقلام: {itemsText}"
            );

            // نوتیفیکیشن
            await _notificationService.CreateAsync(
                data.UserId,
                "پرداخت موفق",
                $"سفارش {data.OrderNumber} پرداخت شد",
                NotificationType.OrderPaid,
                data.BranchId
            );

            return RedirectToAction(
                "Success",
                new { trackingNumber = data.TrackingNumber }
            );
        }

        // =======================
        // صفحه موفق
        // =======================
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Success(long trackingNumber)
        {
            ViewBag.TrackingNumber = trackingNumber;
            return View();
        }
    }
}
