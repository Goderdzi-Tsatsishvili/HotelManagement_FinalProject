
using HotelManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Application.Contracts.Persistence
{
    public interface IRoomRepository : IRepositoryBase<Room, DbContext>
    {
    }
}
