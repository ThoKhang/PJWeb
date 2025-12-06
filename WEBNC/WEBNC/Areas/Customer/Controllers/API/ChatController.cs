using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WEBNC.DataAccess.Repository.IRepository;
using WEBNC.Models;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Linq;
using System.Text.RegularExpressions;

[ApiController]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly HttpClient _http;

    public ChatController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _http = new HttpClient();
    }

    [HttpPost("send")]
    [Authorize]
    public async Task<IActionResult> Send([FromBody] ChatRequest req)
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        string userMessage = req.Message.ToLower().Trim();

        // 1. Lấy session hoặc tạo mới
        ChatSession session = _unitOfWork.ChatSession
            .GetFirstOrDefault(s => s.userId == userId);

        if (session == null)
        {
            session = new ChatSession
            {
                userId = userId,
                title = "Cuộc trò chuyện mới"
            };
            _unitOfWork.ChatSession.Add(session);
            _unitOfWork.save();
        }

        // 2. Lưu tin nhắn user (Chưa gọi save, sẽ gọi sau)
        var userMsg = new ChatMessage
        {
            idSession = session.idSession,
            role = "user",
            message = req.Message
        };
        _unitOfWork.ChatMessage.Add(userMsg);

        // -------------------------------------------------------------
        // **BƯỚC KIỂM TRA ĐƠN GIẢN VÀ TRẢ LỜI NGAY (KHẮC PHỤC LẠC ĐỀ)**
        // -------------------------------------------------------------

        string[] simpleGreetings = { "hello", "chào", "bạn là ai vậy?", "bạn là ai" };
        bool isSimpleGreeting = simpleGreetings.Contains(userMessage);

        if (isSimpleGreeting)
        {
            string fixedReply = "Chào bạn, tôi có thể tư vấn linh kiện điện tử hoặc hỗ trợ thông tin giỏ hàng, đơn hàng cho bạn không?";

            // Lưu tin nhắn user và phản hồi cố định vào DB
            var aiMsgFixed = new ChatMessage { idSession = session.idSession, role = "ai", message = fixedReply };
            _unitOfWork.ChatMessage.Add(aiMsgFixed);
            _unitOfWork.save();

            // Trả về JSON (Không dùng streaming) để cập nhật nhanh Frontend
            return Ok(new { reply = fixedReply });
        }

        // Nếu không phải câu chào, gọi save cho tin nhắn user và tiếp tục logic phức tạp
        _unitOfWork.save();


        // ==========================================================
        // 3a. XÁC ĐỊNH & TRUY VẤN DỮ LIỆU (OPTIMIZED CONTEXT RETRIEVAL)
        // ==========================================================

        bool isComplexQuery =
            userMessage.Contains("sản phẩm") ||
            userMessage.Contains("giỏ hàng") ||
            userMessage.Contains("đơn hàng") ||
            userMessage.Contains("tư vấn") ||
            userMessage.Contains("giá") ||
            userMessage.Contains("bán chạy") ||
            userMessage.Contains("loại gì");

        string productContext = "";
        string cartContext = "";
        string orderContext = "";
        string categoryContext = "";
        string bestSellerContext = "";

        string searchKeyword = userMessage;


        if (isComplexQuery)
        {
            // --- A. XỬ LÝ TRÍCH XUẤT TÊN SẢN PHẨM/TỪ KHÓA ---
            string extractedName = "";

            if (userMessage.StartsWith("tôi muốn tư vấn sản phẩm") || userMessage.StartsWith("tôi muốn hỏi thông tin của tên sản phẩm"))
            {
                extractedName = userMessage
                    .Replace("tôi muốn tư vấn sản phẩm", "")
                    .Replace("tôi muốn hỏi thông tin của tên sản phẩm", "")
                    .Trim();
            }

            if (string.IsNullOrEmpty(extractedName))
            {
                var match = Regex.Match(userMessage, @"([a-z0-9_\-\.]{4,})");
                if (match.Success)
                {
                    extractedName = match.Groups[1].Value.Trim();
                }
            }

            if (!string.IsNullOrEmpty(extractedName))
            {
                searchKeyword = extractedName;
            }

            // --- B. TRUY VẤN CONTEXT SẢN PHẨM CỤ THỂ ---
            var relevantProducts = _unitOfWork.SanPham
                .GetAll(p => p.tenSanPham.ToLower().Contains(searchKeyword) ||
                             p.moTa.ToLower().Contains(searchKeyword),
                        includeProperties: "LoaiSanPham")
                .Take(3)
                .Select(p => new
                {
                    p.idSanPham,
                    p.tenSanPham,
                    p.gia,
                    p.moTa,
                    CategoryName = p.LoaiSanPham.tenLoaiSanPham
                })
                .ToList();

            if (relevantProducts.Any())
            {
                productContext = JsonConvert.SerializeObject(relevantProducts);
            }


            // --- C. TRUY VẤN CONTEXT GIỎ HÀNG (Nếu từ khóa chứa giỏ hàng) ---
            if (userMessage.Contains("giỏ hàng"))
            {
                var userCart = _unitOfWork.chiTietGioHang
                    .GetAll(c => c.idNguoiDung == userId, includeProperties: "SanPham")
                    .Select(c => new
                    {
                        TenSP = c.SanPham.tenSanPham,
                        MaSP = c.idSanPham,
                        SoLuong = c.soLuongTrongGio,
                        Gia = c.SanPham.gia
                    })
                    .ToList();

                if (userCart.Any())
                {
                    cartContext = JsonConvert.SerializeObject(userCart);
                }
            }


            // --- D. TRUY VẤN CONTEXT ĐƠN HÀNG (Nếu từ khóa chứa đơn hàng) ---
            if (userMessage.Contains("đơn hàng"))
            {
                var recentOrders = _unitOfWork.DonDatHang
                    .GetAll(o => o.idNguoiDung == userId)
                    .OrderByDescending(o => o.ngayDat)
                    .Take(1)
                    .Select(o => new
                    {
                        o.idDonDat,
                        o.trangThai,
                        o.ngayDat,
                        ChiTietSanPham = _unitOfWork.ChiTietDonHang
                            .GetAll(d => d.idDonDat == o.idDonDat, includeProperties: "SanPham")
                            .Select(d => new
                            {
                                MaSP = d.idSanPham,
                                TenSP = d.SanPham.tenSanPham,
                                SoLuong = d.soluong,
                                DonGia = d.donGia
                            })
                    })
                    .ToList();

                if (recentOrders.Any())
                {
                    orderContext = JsonConvert.SerializeObject(recentOrders);
                }
            }

            // --- E. TRUY VẤN CONTEXT CHUNG ---
            if (userMessage.Contains("bán chạy") || userMessage.Contains("giá rẻ") || userMessage.Contains("loại gì"))
            {
                var topProducts = _unitOfWork.SanPham.GetAll()
                    .OrderByDescending(p => p.soLuongHienCon)
                    .Take(3)
                    .Select(p => new { p.tenSanPham, p.gia })
                    .ToList();
                bestSellerContext = JsonConvert.SerializeObject(topProducts);

                var categories = _unitOfWork.LoaiSanPham.GetAll().Select(l => l.tenLoaiSanPham).ToList();
                categoryContext = JsonConvert.SerializeObject(categories);
            }
        }


        // ==========================================================
        // TẠO SYSTEM PROMPT (HƯỚNG DẪN AI)
        // ==========================================================
        string systemPrompt = $@"
            BẠN PHẢI TUYỆT ĐỐI TUÂN THỦ TẤT CẢ CÁC LUẬT VÀ CHỈ DÙNG TIẾNG VIỆT.
            BẠN LÀ TRỢ LÝ CHUYÊN NGHIỆP CỦA WEBSITE LINH KIỆN ĐIỆN TỬ.

            --- DỮ LIỆU NGỮ CẢNH ---
            1. Sản phẩm Cụ thể: {productContext}
            2. Chi tiết Giỏ hàng User: {cartContext}
            3. Đơn hàng Gần nhất User: {orderContext}
            4. SP Bán chạy/Danh mục: {bestSellerContext} - Loại SP: {categoryContext}

            --- LUẬT TRẢ LỜI CỨNG ---
            1. ĐỊNH DẠNG: PHẢI trả lời NGẮN GỌN TỐI ĐA 2 CÂU. KHÔNG ĐƯỢC vượt quá giới hạn này.
            2. NGÔN NGỮ: BẮT BUỘC SỬ DỤNG TIẾNG VIỆT. KHÔNG DÙNG bất kỳ ngôn ngữ nào khác.
            3. Giỏ hàng/Đơn hàng: Chỉ tóm tắt Tên SP, Mã SP, và Trạng thái Thanh toán/Tổng tiền.
            4. Dữ liệu: Chỉ dùng thông tin trong DỮ LIỆU NGỮ CẢNH.
            5. CẤM: KHÔNG ĐƯỢC nhắc đến 'JSON', 'DB', hoặc các chủ đề lạc đề (ví dụ: tiếng Anh, học thuật).
            ";

        // === 3b. Gửi đến AI (Ollama) và STREAMING PHẢN HỒI ===

        Response.ContentType = "application/octet-stream";
        Response.Headers.Add("X-Content-Type-Options", "nosniff");

        var body = new
        {
            model = "phi3:mini",
            prompt = $"{systemPrompt}\n\nUser Question: {req.Message}",
            stream = true
        };

        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost:11434/api/generate");
        requestMessage.Content = JsonContent.Create(body);

        using var response = await _http.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream);

        string aiReply = "";

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(line)) continue;

            try
            {
                var chunk = JsonConvert.DeserializeObject<dynamic>(line);
                if (chunk != null && chunk.response != null)
                {
                    string part = (string)chunk.response;
                    aiReply += part;

                    var bytes = System.Text.Encoding.UTF8.GetBytes(part);
                    await Response.Body.WriteAsync(bytes);
                    await Response.Body.FlushAsync();
                }
            }
            catch
            {
                // bỏ qua chunk lỗi
            }
        }

        // === 4. LƯU TIN NHẮN AI VÀO DB SAU KHI STREAM XONG ===
        var aiMsg = new ChatMessage
        {
            idSession = session.idSession,
            role = "ai",
            message = aiReply
        };

        _unitOfWork.ChatMessage.Add(aiMsg);
        _unitOfWork.save();

        return new EmptyResult();
    }

    // Lấy lịch sử chat (Giữ nguyên)
    [HttpGet("history")]
    [Authorize]
    public IActionResult History()
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var session = _unitOfWork.ChatSession.GetFirstOrDefault(s => s.userId == userId);

        if (session == null)
            return Ok(new List<ChatMessage>());

        var messages = _unitOfWork.ChatMessage
            .GetAll(m => m.idSession == session.idSession)
            .OrderBy(m => m.idMessage)
            .ToList();

        return Ok(messages);
    }
}

public class ChatRequest
{
    public string Message { get; set; }
}