using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEBNC.Models;

namespace WEBNC.Areas.Admin.Controllers.API
{
    [Area("Admin")]
    [Route("api/admin/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userManager.Users.Include(u => u.xaPhuong).ToListAsync();
            var result = new List<object>();
            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);

                result.Add(new
                {
                    id = u.Id,
                    userName = u.UserName,
                    email = u.Email,
                    phone = u.PhoneNumber,
                    diaChi = $"{u.soNha}, {u.xaPhuong?.tenXaPhuong}",
                    role = roles.FirstOrDefault(),
                    emailConfirmed = u.EmailConfirmed,
                    isOtpVerified = u.IsOtpVerified
                });
            }

            return Ok(result);
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] AdminCreateUser dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserName) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new { message = "Thiếu thông tin bắt buộc" });
            var exists = await _userManager.FindByNameAsync(dto.UserName);
            if (exists != null)
                return BadRequest(new { message = "UserName đã tồn tại" });
            var user = new ApplicationUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                EmailConfirmed = true,
                IsOtpVerified = true
            };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(new{message = string.Join(", ",result.Errors.Select(e => e.Description))});
            if (!string.IsNullOrEmpty(dto.RoleName))
                await _userManager.AddToRoleAsync(user, dto.RoleName);
            return Ok(new { success = true });
        }

    }
}
