
using HotelManagement.Application.Models.Common;
using HotelManagement.Application.Models.Hotel;

namespace HotelManagement.Application.Contracts.Service
{
    public interface IHotelService
    {
        Task<int> CreateNewHotelAsync(HotelForCreatingDto model);
        Task<HotelForGettingDto> GetHotelAsync(int hotelId);
        Task<int> DeleteHotelAsync(int hotelId);
        Task<int> UpdateHotelAsync(HotelForUpdatingDto model);
        Task<PagedResponseDto<HotelListForGettingDto>> GetAllHotelsAsync(PagedRequestDto parameters);
        Task<PagedResponseDto<HotelListForGettingDto>> GetAllHotelsOfCountryAsync(string countryName, PagedRequestDto parameters);
        Task<PagedResponseDto<HotelListForGettingDto>> GetAllHotelsOfCityAsync(string cityName, PagedRequestDto parameters);
        Task<PagedResponseDto<HotelListForGettingDto>> GetAllHotelsOfRatingAsync(int rating, PagedRequestDto parameters);
    }
}
