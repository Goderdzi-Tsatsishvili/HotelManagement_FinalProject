
using HotelManagement.Application.Contracts.Persistence;
using HotelManagement.Domain.Entities;
using HotelManagement.Infrastructure.Data;

namespace HotelManagement.Infrastructure.Persistence
{
    public class ReservationRepository : RepositoryBase<Reservation, AppDbContext>, IReservationRepository
    {
        public ReservationRepository(AppDbContext options) : base(options)
        {
            
        }
    }
}
