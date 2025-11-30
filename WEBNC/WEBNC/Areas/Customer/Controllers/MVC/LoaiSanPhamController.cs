using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace WEBNC.Areas.Customer.Controllers.MVC
{
    [Area("Customer")]
    public class LoaiSanPhamController : Controller
    {
        private readonly HttpClient _http;

        public LoaiSanPhamController(IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory.CreateClient();
            _http.BaseAddress = new Uri("https://localhost:7047/");
        }

        public async Task<IActionResult> Index()
        {
            var res = await _http.GetAsync("api/products/loai");

            if (!res.IsSuccessStatusCode)
            {
                ViewBag.Error = "Không lấy được loại sản phẩm";
                return View(new List<object>());
            }

            var json = await res.Content.ReadAsStringAsync();
            var data = JObject.Parse(json)["data"].ToObject<List<object>>();

            return View(data);
        }
    }
}
