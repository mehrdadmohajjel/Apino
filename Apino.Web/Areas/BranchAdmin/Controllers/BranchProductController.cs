using Apino.Application.Dtos.ProductDto;
using Apino.Application.Services.Products;
using Microsoft.AspNetCore.Mvc;

namespace Apino.Web.Areas.BranchAdmin.Controllers
{
    [Route("BranchAdmin/Products")]
    public class BranchProductController : BranchAdminBaseController
    {
        private readonly IProductService _service;

        public BranchProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet("")]
        public IActionResult Index() => View();

        [HttpGet("list")]
        public async Task<IActionResult> List(
          [FromQuery] int page = 1,
          [FromQuery] long? categoryId = null,
          [FromQuery] long? serviceTypeId = null)
        {
            var branchId = User.GetBranchId();
            // ارسال پارامترهای فیلتر به سرویس
            var result = await _service.GetByBranchAsync(branchId, page, 10, categoryId, serviceTypeId);
            return Json(result);
        }

        [HttpGet("form-data")]
        public async Task<IActionResult> GetFormData()
        {
            var branchId = User.GetBranchId();
            var data = await _service.GetDropdownsAsync(branchId);
            return Json(data);
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromForm] SaveProductVm vm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var branchId = User.GetBranchId();

            if (vm.Id == 0)
                await _service.CreateAsync(branchId, vm);
            else
                await _service.UpdateAsync(branchId, vm);

            return Ok();
        }

        [HttpPost("toggle")]
        public async Task<IActionResult> Toggle([FromBody] long id)
        {
            await _service.ToggleActiveAsync(id);
            return Ok();
        }
    }
}