
using HotelManagement.Application.Contracts.Persistence;
using HotelManagement.Domain.Entities;
using HotelManagement.Infrastructure.Data;

namespace HotelManagement.Infrastructure.Persistence
{
    public class RefreshTokenRepository : RepositoryBase<RefreshToken, AppDbContext>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(AppDbContext context) : base(context)
        {
            
        }
    }
}
