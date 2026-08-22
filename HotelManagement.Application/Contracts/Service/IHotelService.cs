
using HotelManagement.Application.Models.Common;
using HotelManagement.Application.Models.Hotel;

namespace HotelManagement.Application.Contracts.Service
{
    public interface IHotelService
    {
        Task<int> CreateNewHotelAsync(HotelForCreatingDto model);
        Task<HotelForGettingDto> GetHotelAsync(int hotelId);
        Task<int> DeleteHotelAsync(int hotelId);
        Task<int> UpdateHotelAsync(int hotelId, HotelForUpdatingDto model);
        Task<PagedResponseDto<HotelListForGettingDto>> GetAllHotelsAsync(PagedRequestDto parameters);
        Task<PagedResponseDto<HotelListForGettingDto>> GetAllHotelsBySearchParamsAsync(string? countryName, string? city, int? rating, PagedRequestDto parameters);
        Task<HotelWithManagerForGettingDto> GetHotelWithManagerAsync(int? hotelId);
    }
}
