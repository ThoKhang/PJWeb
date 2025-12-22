using Microsoft.AspNetCore.Mvc;

namespace WEBNC.Areas.Admin.Controllers.API
{
    [Area("Admin")]
    [Route("api/admin/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        [HttpGet("summary")]
        public IActionResult GetSummary()
        {
            // TODO: sau này lấy từ DB
            var data = new
            {
                TodayRevenue = 12500000,
                TodayOrders = 48,
                NewUsers7Days = 112,
                TotalActiveProducts = 320
            };
            return Ok(data);
        }

        [HttpGet("revenue-chart")]
        public IActionResult GetRevenueChart()
        {
            var data = new
            {
                labels = new[] { "Th1", "Th2", "Th3", "Th4", "Th5", "Th6", "Th7", "Th8", "Th9", "Th10", "Th11", "Th12" },
                values = new[] { 12, 9, 15, 18, 22, 19, 25, 28, 24, 30, 32, 35 }
            };
            return Ok(data);
        }
    }
}
