using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WEBNC.DataAccess.Repository.IRepository;
using WEBNC.Models;
using Newtonsoft.Json;
using System.Net.Http.Json; // Cần thiết cho PostAsJsonAsync hoặc tạo JsonContent

[ApiController]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly HttpClient _http;

    // Khởi tạo HttpClient trong constructor (được khuyến nghị hơn)
    public ChatController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        // Khởi tạo HttpClient một lần
        _http = new HttpClient();
    }

    [HttpPost("send")]
    [Authorize]
    public async Task<IActionResult> Send([FromBody] ChatRequest req)
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // 1. Lấy session hoặc tạo mới
        // Khai báo biến 'session' ở đây để có phạm vi (scope) toàn bộ phương thức
        ChatSession session = _unitOfWork.ChatSession
            .GetFirstOrDefault(s => s.userId == userId);

        if (session == null)
        {
            // Chỉ gán giá trị, KHÔNG DÙNG 'var' lần nữa
            session = new ChatSession
            {
                userId = userId,
                title = "Cuộc trò chuyện mới"
            };

            _unitOfWork.ChatSession.Add(session);
            _unitOfWork.save();
        }

        // 2. Lưu tin nhắn user (Sử dụng session đã được gán giá trị ở bước 1)
        var userMsg = new ChatMessage
        {
            idSession = session.idSession, // Đã khắc phục lỗi CS0103
            role = "user",
            message = req.Message
        };

        _unitOfWork.ChatMessage.Add(userMsg);
        _unitOfWork.save();


        // === 3. Gửi đến AI (Ollama) và STREAMING PHẢN HỒI (Cải thiện tốc độ) ===

        // Thiết lập header để phản hồi dạng stream/raw
        Response.ContentType = "application/octet-stream";
        Response.Headers.Add("X-Content-Type-Options", "nosniff");

        var body = new
        {
            model = "phi3:mini",
            prompt = req.Message,
            stream = true // QUAN TRỌNG
        };

        // Gửi Request và nhận phản hồi
        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost:11434/api/generate");
        requestMessage.Content = JsonContent.Create(body);

        using var response = await _http.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream);

        string aiReply = ""; // Chuỗi này sẽ tổng hợp toàn bộ câu trả lời

        // Đọc từng dòng (chunk) và stream về Frontend
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

                    // Ghi phần phản hồi (chunk) trực tiếp về Response của Client
                    var bytes = System.Text.Encoding.UTF8.GetBytes(part);
                    await Response.Body.WriteAsync(bytes);
                    await Response.Body.FlushAsync(); // Đẩy dữ liệu ra ngay lập tức
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
            message = aiReply // Dùng chuỗi đã tổng hợp
        };

        _unitOfWork.ChatMessage.Add(aiMsg);
        _unitOfWork.save();

        // Trả về một kết quả rỗng (vì dữ liệu đã được stream hết)
        return new EmptyResult();
    }

    // Lấy lịch sử chat
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
            .OrderBy(m => m.idMessage) // DÙNG idMessage để order
            .ToList();

        return Ok(messages);
    }
}

public class ChatRequest
{
    public string Message { get; set; }
}