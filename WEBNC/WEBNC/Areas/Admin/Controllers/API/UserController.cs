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
    }
}
