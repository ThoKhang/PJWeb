using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using WEBNC.Services.VnPay;
using WEBNC.ViewModels.VnPay;

namespace WEBNC.Areas.Customer.Controllers
{
    public class PaymentController : Controller
    {

        private readonly IVnPayService _vnPayService;
        public PaymentController(IVnPayService vnPayService)
        {

            _vnPayService = vnPayService;
        }

        public IActionResult CreatePaymentUrlVnpay(PaymentInformationModel model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            return Redirect(url);
        }
       


    }

}
