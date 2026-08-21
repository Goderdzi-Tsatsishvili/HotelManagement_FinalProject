
using HotelManagement.Application.Models.Common;
using HotelManagement.Application.Models.Reservation;

namespace HotelManagement.Application.Contracts.Service
{
    public interface IReservationService
    {
        Task<int> CreateReservationAsync(int hotelId, ReservationForCreatingDto model, string guestId);
        Task<ReservationForGettingDto> GetReservationAsync(int hotelId, int reservationId);
        Task<PagedResponseDto<ReservationListForGettingDto>> GetReservationsOfHotelAsync(int hotelId, PagedRequestDto parameters);
        Task<PagedResponseDto<ReservationListForGettingDto>> GetReservationsBySearchParamsAsync(
            PagedRequestDto parameters,
            int? hotelId,
            string? guestId,
            int? roomId,
            DateTime? date);
        Task<int> UpdateReservationAsync(int hotelId, ReservationForUpdatingDto model);
        Task<int> DeleteReservationAsync(int hotelId, int reservationId);
    }
}
