using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WEBNC.Models;

namespace WEBNC.Areas.Customer.Controllers.MVC
{
    [Area("Customer")]
    [Authorize]
    public class Cart : Controller
    {
        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _accessor;

        public Cart(IHttpClientFactory httpClientFactory, IHttpContextAccessor accessor)
        {
            _http = httpClientFactory.CreateClient();
            _http.BaseAddress = new Uri("https://localhost:7047/");
            _accessor = accessor;
        }

        public async Task<IActionResult> Index()
        {
            // gửi cookie identity cùng request
            var cookie = _accessor.HttpContext!.Request.Headers["Cookie"].ToString();
            if (!string.IsNullOrEmpty(cookie))
            {
                _http.DefaultRequestHeaders.Remove("Cookie");
                _http.DefaultRequestHeaders.Add("Cookie", cookie);
            }

            var response = await _http.GetAsync("api/cart");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return Redirect("/Identity/Account/Login?returnUrl=/Customer/Cart");
            }

            var json = await response.Content.ReadAsStringAsync();

            // Parse JSON mà không cần tạo class
            using var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("data", out var dataElem)
                || dataElem.ValueKind != JsonValueKind.Array)
            {
                return View(new List<ChiTietGioHang>()); // fallback
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var cartList = JsonSerializer.Deserialize<List<ChiTietGioHang>>(dataElem.GetRawText(), options)
                           ?? new List<ChiTietGioHang>();

            return View(cartList);
        }
    }
}
