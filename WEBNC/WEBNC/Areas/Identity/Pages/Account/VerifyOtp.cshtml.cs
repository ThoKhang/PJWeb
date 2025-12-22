using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEBNC.Models;
using WEBNC.Utility;

namespace WEBNC.Areas.Identity.Pages.Account
{
    public class VerifyOtpModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public VerifyOtpModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public string OtpInput { get; set; }

        public string ErrorMessage { get; set; }

        public string SuccessMessage { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            var otp = HttpContext.Session.GetString("OTP_CODE");
            var email = HttpContext.Session.GetString("OTP_EMAIL");
            var expireStr = HttpContext.Session.GetString("OTP_EXPIRE");

            if (otp == null || email == null || expireStr == null)
            {
                ErrorMessage = "OTP không tồn tại hoặc đã hết hạn.";
                return Page();
            }

            if (DateTime.Now > DateTime.Parse(expireStr))
            {
                ErrorMessage = "OTP đã hết hạn.";
                return Page();
            }

            if (OtpInput != otp)
            {
                ErrorMessage = "OTP không đúng.";
                return Page();
            }
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                ErrorMessage = "Không tìm thấy tài khoản.";
                return Page();
            }

            user.IsOtpVerified = true;
            await _userManager.UpdateAsync(user);

            await _signInManager.SignInAsync(user, isPersistent: false);

            HttpContext.Session.Remove("OTP_CODE");
            HttpContext.Session.Remove("OTP_EMAIL");
            HttpContext.Session.Remove("OTP_EXPIRE");

            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }
        public async Task<IActionResult> OnPostResendAsync()
        {
            var email = HttpContext.Session.GetString("OTP_EMAIL");

            if (string.IsNullOrEmpty(email))
            {
                ErrorMessage = "Phiên xác thực đã hết hạn. Vui lòng đăng ký lại.";
                return Page();
            }

            var newOtp = new Random().Next(100000, 999999).ToString();

            HttpContext.Session.SetString("OTP_CODE", newOtp);
            HttpContext.Session.SetString(
                "OTP_EXPIRE",
                DateTime.Now.AddMinutes(5).ToString("yyyy-MM-dd HH:mm:ss")
            );
            await _emailSender.SendEmailAsync(
                email,
                "Mã OTP mới",
                $@"
                <p>Mã OTP mới của bạn là:</p>
                <h2 style='color:#0d6efd'>{newOtp}</h2>
                <p>Mã có hiệu lực trong 5 phút.</p>
                "
            );

            SuccessMessage = "Đã gửi lại mã OTP mới. Vui lòng kiểm tra email.";

            return Page();
        }
    }
}
