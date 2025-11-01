using System;
using Web.DataAccess.Repository.IRepository;
using WEBNC.Models;

namespace WEBNC.DataAccess.Repository.IRepository
{
    public interface ISanPhamRepository: IRepository<SanPham>
    {
        void Update(SanPham obj);
        IEnumerable<SanPham> LayTop10SanPhamBanChay();
    }
}
