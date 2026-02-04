using Apino.Application.Common.Helper;
using Apino.Application.Services.Payments;
using Microsoft.AspNetCore.Mvc;

namespace Apino.Web.Areas.BranchAdmin.Controllers
{
    [Route("BranchAdmin/PaymentReports")]
    public class BranchPaymentReportsController : BranchAdminBaseController
    {
        private readonly IPaymentReportService _service;

        public BranchPaymentReportsController(IPaymentReportService service)
        {
            _service = service;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            // تاریخ امروز را برای پیش‌فرض در ویو ارسال می‌کنیم
            ViewBag.TodayDate = PersianDateHelper.ToShamsi(DateTime.Now);
            return View();
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList(int page = 1, string fromDate = "", string toDate = "")
        {
            var branchId = User.GetBranchId();

            // اگر کاربر تاریخی نفرستاد، پیش‌فرض امروز در نظر گرفته شود
            if (string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate))
            {
                var today = PersianDateHelper.ToShamsi(DateTime.Now);
                fromDate = today;
                toDate = today;
            }

            var result = await _service.GetBranchPaymentsAsync(branchId, fromDate, toDate, page);
            return Json(result);
        }
    }
}