using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;
using WEBNC.Models;
using WEBNC.Models.Momo;

namespace WEBNC.Services.Momo
{
    public class MomoService : IMomoService
    {
        private readonly IOptions<MomoOptionModel> _options;
        public MomoService(IOptions<MomoOptionModel> options)
        {
            _options = options;
        }
        public async Task<MomoCreatePaymentResponseModel> CreatePaymentMomo(OrderInfoModel model, string? returnUrlOverride = null, string? notifyUrlOverride = null)
        {
            var userIdForExtra = model.OrderInfo;
            model.OrderId = DateTime.UtcNow.Ticks.ToString();
            var orderInfoText = "Thanh toán đơn hàng của " + model.FullName;
            var amountStr = Convert.ToInt64(model.Amount).ToString(CultureInfo.InvariantCulture);
            var returnUrl = string.IsNullOrEmpty(returnUrlOverride) ? _options.Value.ReturnUrl : returnUrlOverride;
            var notifyUrl = string.IsNullOrEmpty(notifyUrlOverride) ? _options.Value.NotifyUrl : notifyUrlOverride;
            var rawData =
                $"partnerCode={_options.Value.PartnerCode}" +
                $"&accessKey={_options.Value.AccessKey}" +
                $"&requestId={model.OrderId}" +
                $"&amount={amountStr}" +
                $"&orderId={model.OrderId}" +
                $"&orderInfo={orderInfoText}" +
                $"&returnUrl={returnUrl}" +
                $"&notifyUrl={notifyUrl}" +
                $"&extraData={userIdForExtra}";
            var signature = ComputeHmacSha256(rawData, _options.Value.SecretKey);
            var client = new RestClient(_options.Value.MomoApiUrl);
            var request = new RestRequest() { Method = RestSharp.Method.Post };
            request.AddHeader("Content-Type", "application/json; charset=UTF-8");
            var requestData = new
            {
                accessKey = _options.Value.AccessKey,
                partnerCode = _options.Value.PartnerCode,
                requestType = _options.Value.RequestType,
                notifyUrl = notifyUrl,
                returnUrl = returnUrl,
                orderId = model.OrderId,
                amount = amountStr,
                orderInfo = orderInfoText,
                requestId = model.OrderId,
                extraData = userIdForExtra,
                signature = signature
            };
            request.AddJsonBody(requestData);
            var response = await client.ExecuteAsync(request);
            var momoResponse = JsonConvert.DeserializeObject<MomoCreatePaymentResponseModel>(response.Content);
            if (momoResponse == null || string.IsNullOrEmpty(momoResponse.PayUrl))
            {
                var v2Client = new RestClient("https://test-payment.momo.vn/v2/gateway/api/create");
                var v2Request = new RestRequest() { Method = RestSharp.Method.Post };
                v2Request.AddHeader("Content-Type", "application/json; charset=UTF-8");
                var v2Raw =
                $"accessKey={_options.Value.AccessKey}" +
                $"&amount={amountStr}" +
                $"&extraData={userIdForExtra}" +
                $"&ipnUrl={notifyUrl}" +
                $"&orderId={model.OrderId}" +
                $"&orderInfo={orderInfoText}" +
                $"&partnerCode={_options.Value.PartnerCode}" +
                $"&redirectUrl={returnUrl}" +
                $"&requestId={model.OrderId}" +
                $"&requestType=captureWallet";
                var v2Sig = ComputeHmacSha256(v2Raw, _options.Value.SecretKey);
                var v2Data = new
                {
                    partnerCode = _options.Value.PartnerCode,
                    requestType = "captureWallet",
                    ipnUrl = notifyUrl,
                    redirectUrl = returnUrl,
                    orderId = model.OrderId,
                    amount = amountStr,
                    orderInfo = orderInfoText,
                    requestId = model.OrderId,
                    extraData = userIdForExtra,
                    signature = v2Sig,
                    accessKey = _options.Value.AccessKey
                };
                v2Request.AddParameter("application/json", JsonConvert.SerializeObject(v2Data), ParameterType.RequestBody);
                var v2Resp = await v2Client.ExecuteAsync(v2Request);
                var v2Parsed = JsonConvert.DeserializeObject<MomoCreatePaymentResponseModel>(v2Resp.Content);
                return v2Parsed;
            }
            return momoResponse;


        }
        public MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection)
        {
            string Get(string key) => collection.FirstOrDefault(s => s.Key == key).Value;
            var amount = Get("amount");
            var orderInfo = Get("orderInfo");
            var orderId = Get("orderId");
            var errorCode = Get("errorCode");
            var resultCode = Get("resultCode");
            var message = Get("message");
            var localMessage = Get("localMessage");
            var transId = Get("transId");
            var payType = Get("payType");
            var responseTime = Get("responseTime");
            var signature = Get("signature");
            var extraData = Get("extraData");

            return new MomoExecuteResponseModel()
            {
                Amount = amount,
                OrderId = orderId,
                OrderInfo = orderInfo,
                ErrorCode = errorCode,
                ResultCode = resultCode,
                Message = message,
                LocalMessage = localMessage,
                TransId = transId,
                PayType = payType,
                ResponseTime = responseTime,
                Signature = signature,
                ExtraData = extraData
            };
        }

        private string ComputeHmacSha256(string message, string SecretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(SecretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);
            byte[] hashBytes;
            using (var hmac = new HMACSHA256(keyBytes))
            {
                hashBytes = hmac.ComputeHash(messageBytes);
            }
            var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
              return hashString;
        }



    }
}
