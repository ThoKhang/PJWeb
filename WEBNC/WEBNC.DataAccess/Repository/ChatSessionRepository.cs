using WEBNC.Models;
using WEBNC.DataAccess.Repository.IRepository;

namespace WEBNC.DataAccess.Repository
{
    public class ChatSessionRepository : Repository<ChatSession>, IChatSessionRepository
    {
        private readonly ApplicationDbContext _db;

        public ChatSessionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ChatSession session)
        {
            _db.ChatSessions.Update(session);
        }
    }
}

