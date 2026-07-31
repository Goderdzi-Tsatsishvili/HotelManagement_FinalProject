
using HotelManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Application.Contracts.Persistence
{
    public interface IHotelRepository : IRepositoryBase<Hotel, DbContext>
    {
    }
}
