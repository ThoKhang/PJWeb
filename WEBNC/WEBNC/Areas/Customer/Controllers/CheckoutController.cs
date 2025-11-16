using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using WEBNC.Services.VnPay;

namespace WEBNC.Areas.Customer.Controllers
{
    public class CheckoutController: Controller
    {
        public CheckoutController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }
        private readonly IVnPayService _vnPayService;
        [HttpGet]
        public IActionResult PaymentCallbackVnpay()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            return Json(response);
        }
    }
}
