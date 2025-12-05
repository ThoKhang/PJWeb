using WEBNC.Models;
using WEBNC.DataAccess.Repository.IRepository;

namespace WEBNC.DataAccess.Repository
{
    public class ChatMessageRepository : Repository<ChatMessage>, IChatMessageRepository
    {
        private readonly ApplicationDbContext _db;

        public ChatMessageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ChatMessage message)
        {
            _db.ChatMessages.Update(message);
        }
    }
}
