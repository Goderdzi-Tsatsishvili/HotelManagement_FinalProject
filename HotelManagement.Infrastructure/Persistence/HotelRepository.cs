
using HotelManagement.Application.Contracts.Persistence;
using HotelManagement.Domain.Entities;
using HotelManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Infrastructure.Persistence
{
    public class HotelRepository : RepositoryBase<Hotel, AppDbContext>, IHotelRepository
    {
        public HotelRepository(AppDbContext options) : base(options)
        {
            
        }
    }
}
