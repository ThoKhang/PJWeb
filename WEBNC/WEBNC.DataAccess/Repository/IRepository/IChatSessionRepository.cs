using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WEBNC.Models;

namespace WEBNC.DataAccess.Repository.IRepository
{
    public interface IChatSessionRepository : IRepository<ChatSession>
    {
        void Update(ChatSession session);
    }
}

