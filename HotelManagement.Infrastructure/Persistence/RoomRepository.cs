
using HotelManagement.Application.Contracts.Persistence;
using HotelManagement.Domain.Entities;
using HotelManagement.Infrastructure.Data;

namespace HotelManagement.Infrastructure.Persistence
{
    public class RoomRepository : RepositoryBase<Room, AppDbContext>, IRoomRepository
    {
        public RoomRepository(AppDbContext options) : base(options)
        {
            
        }
    }
}
