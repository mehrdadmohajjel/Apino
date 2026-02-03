using Apino.Application.Dtos.BranchStaff;
using Apino.Application.Services.BranchStaff;
using Microsoft.AspNetCore.Mvc;

namespace Apino.Web.Areas.BranchAdmin.Controllers
{
    [Route("BranchAdmin/Staff")]
    public class BranchStaffController : BranchAdminBaseController
    {
        private readonly IBranchStaffService _service;

        public BranchStaffController(IBranchStaffService service)
        {
            _service = service;
        }

        [HttpGet("")]
        public IActionResult Index() => View();

        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            var branchId = User.GetBranchId();
            var data = await _service.GetStaffByBranchAsync(branchId);
            return Json(data);
        }

        [HttpGet("search-users")]
        public async Task<IActionResult> SearchUsers([FromQuery] string term)
        {
            var result = await _service.SearchUsersAsync(term);
            return Json(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddStaffVm vm)
        {
            var branchId = User.GetBranchId();
            // اگر تاریخ ارسال نشده بود، تاریخ امروز را ست کن
            if (vm.StartWorkDate == default) vm.StartWorkDate = DateTime.Now;

            await _service.AddStaffAsync(branchId, vm);
            return Ok();
        }

        [HttpPost("toggle")]
        public async Task<IActionResult> Toggle([FromBody] long id)
        {
            await _service.ToggleActiveAsync(id);
            return Ok();
        }

        [HttpPost("remove")]
        public async Task<IActionResult> Remove([FromBody] long id)
        {
            await _service.RemoveStaffAsync(id);
            return Ok();
        }
    }
}
