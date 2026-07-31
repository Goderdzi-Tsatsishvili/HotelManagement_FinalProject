
using HotelManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Application.Contracts.Persistence
{
    public interface IReservationRepository : IRepositoryBase<Reservation, DbContext>
    {
    }
}
