using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WEBNC.DataAccess.Repository.IRepository;
using WEBNC.Models;
using Newtonsoft.Json;

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

    // Gửi tin nhắn
    [HttpPost("send")]
    [Authorize]
    public async Task<IActionResult> Send([FromBody] ChatRequest req)
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // 1. Lấy session hoặc tạo mới
        var session = _unitOfWork.ChatSession
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

            // Tạo SYSTEM MESSAGE để AI nói tiếng Việt
            _unitOfWork.ChatMessage.Add(new ChatMessage
            {
                idSession = session.idSession,
                role = "system",
                message = "Bạn là trợ lý AI nói tiếng Việt. Hãy trả lời rõ ràng, dễ hiểu và tự nhiên."
            });

            _unitOfWork.save();
        }

        // 2. Lưu tin nhắn user
        _unitOfWork.ChatMessage.Add(new ChatMessage
        {
            idSession = session.idSession,
            role = "user",
            message = req.Message
        });
        _unitOfWork.save();

        // 3. LẤY TOÀN BỘ LỊCH SỬ CHAT ĐỂ GỬI CHO AI
        var history = _unitOfWork.ChatMessage
            .GetAll(m => m.idSession == session.idSession)
            .OrderBy(m => m.idMessage)
            .ToList();

        // Ghép hội thoại thành 1 prompt lớn
        string fullPrompt = "";

        foreach (var msg in history)
        {
            fullPrompt += $"{msg.role}: {msg.message}\n";
        }

        fullPrompt += "assistant: ";

        // 4. Gửi PROMPT sang Ollama
        var body = new
        {
            model = "phi3:mini",
            prompt = fullPrompt,
            stream = true
        };

        var response = await _http.PostAsJsonAsync("http://localhost:11434/api/generate", body);
        var raw = await response.Content.ReadAsStringAsync();

        string aiReply = "";

        // 5. Xử lý streaming JSON từng dòng của Ollama
        foreach (var line in raw.Split('\n'))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            try
            {
                var chunk = JsonConvert.DeserializeObject<dynamic>(line);
                if (chunk != null && chunk.response != null)
                    aiReply += (string)chunk.response;
            }
            catch
            {
                // bỏ chunk lỗi
            }
        }

        // 6. Lưu tin nhắn AI
        _unitOfWork.ChatMessage.Add(new ChatMessage
        {
            idSession = session.idSession,
            role = "ai",
            message = aiReply
        });

        _unitOfWork.save();

        return Ok(new { reply = aiReply });
    }

    // Lịch sử chat
    [HttpGet("history")]
    [Authorize]
    public IActionResult History()
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var session = _unitOfWork.ChatSession
            .GetFirstOrDefault(s => s.userId == userId);

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
