using Apino.Application.Services.Order;
using Microsoft.AspNetCore.Mvc;

namespace Apino.Web.Areas.BranchAdmin.Controllers
{
    [Route("BranchAdmin/Orders")]
    public class BranchOrdersController : BranchAdminBaseController
    {
        private readonly IOrderService _service;

        public BranchOrdersController(IOrderService service)
        {
            _service = service;
        }

        [HttpGet("")]
        public IActionResult Index() => View();

        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            var branchId = User.GetBranchId();
            var data = await _service.GetOrdersAsync(branchId);
            return Json(data);
        }

        [HttpPost("change-status")]
        public async Task<IActionResult> ChangeStatus([FromBody] long orderId)
        {
            try
            {
                var branchId = User.GetBranchId();
                var userId = User.GetUserID(); // شناسه کاربر لاگین شده در سیستم

                await _service.ChangeStatusAsync(orderId, branchId, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}