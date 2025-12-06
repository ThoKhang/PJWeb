using System.Net.Http.Json;
using WEBNC.DataAccess.Repository.IRepository;

namespace WEBNC.DataAccess.Data
{
    public class ChatManager
    {
        private readonly HttpClient _http;
        private readonly IUnitOfWork _unitOfWork;

        public ChatManager(IUnitOfWork unitOfWork)
        {
            _http = new HttpClient();
            _unitOfWork = unitOfWork;
        }

        public async Task<string> AskAI(string prompt)
        {
            var body = new
            {
                model = "phi3:mini",
                prompt = prompt
            };

            var response = await _http.PostAsJsonAsync("http://localhost:11434/api/generate", body);
            var json = await response.Content.ReadAsStringAsync();

            dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            return (string)result.response;
        }
    }
}
