
using HotelManagement.Application.Contracts.Persistence;
using HotelManagement.Domain.Entities;
using HotelManagement.Infrastructure.Data;

namespace HotelManagement.Infrastructure.Persistence
{
    public class ReservationRoomRepository : RepositoryBase<ReservationRoom, AppDbContext>, IReservationRoomRepository
    {
        public ReservationRoomRepository(AppDbContext options) : base(options)
        {
            
        }
    }
}
