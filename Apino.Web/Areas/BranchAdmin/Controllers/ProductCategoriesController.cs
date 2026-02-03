using Apino.Application.Dtos.ProductCategoryDto;
using Apino.Application.Services.ProdcutCategory;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Apino.Web.Areas.BranchAdmin.Controllers
{
    [Route("BranchAdmin/Categories")]
    public class ProductCategoryController : BranchAdminBaseController
    {
        private readonly IProductCategoryService _service;

        public ProductCategoryController(IProductCategoryService service)
        {
            _service = service;
        }

        // صفحه اصلی
        [HttpGet("")]
        public IActionResult Index() => View();

        // دریافت لیست (اصلاح آدرس برای هماهنگی با JS)
        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            // نکته: متد User.GetBranchId() باید اکستنشن متدی باشد که خودتان نوشتید
            // اگر کار نمی‌کند فعلا هاردکد کنید یا کلیم‌ها را چک کنید
            var branchId = User.GetBranchId();
            var data = await _service.GetByBranchAsync(branchId);
            return Json(data);
        }

        // ذخیره و آپلود (مهم: استفاده از FromForm)
        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromForm] SaveCategoryVm vm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var branchId = User.GetBranchId();

            if (vm.Id == 0)
                await _service.CreateAsync(branchId, vm.CategoryTitle, vm.PayAtPlace, vm.Icon);
            else
                await _service.UpdateAsync(vm.Id, vm.CategoryTitle, vm.PayAtPlace, vm.IsActive, vm.Icon);

            return Ok();
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromBody] long id)
        {
            await _service.ToggleActiveAsync(id);
            return Ok();
        }
    }

}