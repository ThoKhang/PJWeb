using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WEBNC.Areas.Admin.Controllers.API
{
    [Area("Admin")]
    [Route("api/admin/roles")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var roles = _roleManager.Roles.Select(r => new{id = r.Id,name = r.Name}).ToList();
            return Ok(roles);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return BadRequest(new { message = "Tên role không hợp lệ" });

            if (await _roleManager.RoleExistsAsync(roleName))
                return BadRequest(new { message = "Role đã tồn tại" });
            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (result.Succeeded)
                return Ok(new { success = true });

            return BadRequest(new { message = "Tạo role thất bại" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound(new { message = "Role không tồn tại" });

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
                return Ok(new { success = true });

            return BadRequest(new { message = "Không thể xóa role" });
        }
    }
}
