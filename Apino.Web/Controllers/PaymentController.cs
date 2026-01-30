using Apino.Application.Dtos.Payment;
using Apino.Application.Services.Auth;
using Apino.Application.Services.Notif;
using Apino.Application.Services.Order;
using Apino.Domain.Entities;
using Apino.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Parbad;
using Parbad.AspNetCore;

namespace Apino.Web.Controllers
{
    public class PaymentController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly IOrderService _orderService;
        private readonly ISmsSender _sms;
        private readonly IOnlinePayment _onlinePayment;

        public PaymentController(IOrderService orderService,ISmsSender sms, INotificationService notificationService)
        {
            _orderService = orderService;
            _sms = sms;
            _notificationService = notificationService;
        }
        [HttpGet]
        public async Task<IActionResult> Pay(long orderId)
        {
            var order = await _orderService.GetOrderForPaymentAsync(orderId);

            var callbackUrl = Url.Action(
                "Verify",
                "Payment",
                new { orderId },
                Request.Scheme);

            var result = await _onlinePayment.RequestAsync(invoice =>
            {
                invoice.SetAmount(order.TotalAmount)
                       .SetCallbackUrl(callbackUrl)
                       .SetGateway("Mellat")
                       .SetTrackingNumber(order.TrackingNumber);
            });

            if (result.IsSucceed)
                return result.GatewayTransporter.TransportToGateway();

            return View("PayRequestError", result);
        }





        [HttpPost]
        public async Task<IActionResult> Pay(PayViewModel vm)
        {
            var callbackUrl = Url.Action("Verify", "Payment", new { orderId = vm.OrderId }, Request.Scheme);

            var result = await _onlinePayment.RequestAsync(invoice =>
            {
                invoice.SetCallbackUrl(callbackUrl)
                       .SetAmount(vm.Amount)
                       .SetGateway("Mellat")
                       .SetTrackingNumber(vm.OrderId);
            });

            if (result.IsSucceed)
                return result.GatewayTransporter.TransportToGateway();

            return View("PayRequestError", result);
        }

        [HttpGet, HttpPost]
        public async Task<IActionResult> Verify(long orderId)
        {
            // 1️⃣ دریافت اطلاعات پرداخت از Parbad
            var invoice = await _onlinePayment.FetchAsync();

            if (invoice == null)
                return View("PaymentFailed");

            // اگر قبلاً بررسی شده
            if (invoice.IsAlreadyVerified)
                return RedirectToAction("Success");

            // فقط در این وضعیت اجازه Verify داریم
            if (invoice.Status != PaymentFetchResultStatus.ReadyForVerifying)
                return View("PaymentFailed");

            // 2️⃣ تایید پرداخت
            var verify = await _onlinePayment.VerifyAsync(invoice);

            if (!verify.IsSucceed)
                return View("PaymentFailed");

            // 3️⃣ ثبت پرداخت در سیستم
            await _orderService.MarkAsPaidAsync(
                orderId,
                (long)PaymentMethod.Online,
                verify.TransactionCode
            );

            // 4️⃣ کاهش موجودی کالاها
            await _orderService.DecreaseProductStockAsync(orderId);

            // 5️⃣ دریافت اطلاعات پیامک
            var data = await _orderService.GetOrderMobileInfo(orderId);

            var itemsText = string.Join("، ",
                data.Items.Select(i => $"{i.Title}×{i.Qty}")
            );

            var userMessage =
        $@"پرداخت شما با موفقیت انجام شد ✅
شماره سفارش: {data.OrderNumber}
مبلغ: {data.TotalAmount:N0} تومان
اقلام: {itemsText}
با سپاس 🌱";

            var branchAdminMessage =
        $@"سفارش پرداخت شد
شماره: {data.OrderNumber}
مبلغ: {data.TotalAmount:N0} تومان";

            var sysAdminMessage =
        $@"پرداخت جدید ثبت شد
سفارش: {data.OrderNumber}
مبلغ: {data.TotalAmount:N0} تومان";

            // 6️⃣ ارسال پیامک‌ها
            if (!string.IsNullOrWhiteSpace(data.UserMobile))
                await _sms.SendAsync(data.UserMobile, userMessage);

            if (!string.IsNullOrWhiteSpace(data.BranchAdminMobile))
                await _sms.SendAsync(data.BranchAdminMobile, branchAdminMessage);

            if (!string.IsNullOrWhiteSpace(data.SysAdmimMobile))
                await _sms.SendAsync(data.SysAdmimMobile, sysAdminMessage);
            //================Notif
            await _notificationService.CreateAsync(
                            data.UserId,
                            "پرداخت موفق",
                            $"سفارش {data.OrderNumber} با موفقیت پرداخت شد",
                            NotificationType.OrderPaid,
                            data.BranchId
                        );
                            await _notificationService.CreateAsync(
                    data.BranchAdminUserId,
                    "پرداخت جدید",
                    $"سفارش {data.OrderNumber} پرداخت شد",
                    NotificationType.OrderPaid,
                    data.BranchId
                );

                await _notificationService.CreateAsync(
                    data.SystemAdminUserId,
                    "پرداخت جدید",
                    $"سفارش {data.OrderNumber} پرداخت شد",
                    NotificationType.OrderPaid,
                    data.BranchId

                );
            // 7️⃣ انتقال به صفحه موفق
            return RedirectToAction("Success");
        }
    }
}
