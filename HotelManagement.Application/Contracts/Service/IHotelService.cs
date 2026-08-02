
using HotelManagement.Application.Models.Hotel;

namespace HotelManagement.Application.Contracts.Service
{
    public interface IHotelService
    {
        Task<int> CreateNewHotelAsync(HotelForCreatingDto model);
        Task<HotelForGettingDto> GetHotelAsync(int hotelId);
        Task<int> DeleteHotelAsync(int hotelId);
    }
}
