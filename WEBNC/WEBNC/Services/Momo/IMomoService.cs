using WEBNC.Models;
using WEBNC.Models.Momo;

namespace WEBNC.Services.Momo
{
    public interface IMomoService 
    {
       Task<MomoCreatePaymentResponseModel> CreatePaymentMomo(OrderInfoModel model, string? returnUrlOverride = null, string? notifyUrlOverride = null);
      
        MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
    }
}
