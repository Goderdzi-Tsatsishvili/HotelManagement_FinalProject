
using HotelManagement.Application.Models.Common;
using HotelManagement.Application.Models.Reservation;

namespace HotelManagement.Application.Contracts.Service
{
    public interface IReservationService
    {
        Task<int> CreateReservationAsync(int hotelId, ReservationForCreatingDto model, string guestId);
        Task<ReservationForGettingDto> GetReservationAsync(int reservationId);
        Task<PagedResponseDto<ReservationListForGettingDto>> GetAllReservationsAsync(PagedRequestDto parameters);
        Task<PagedResponseDto<ReservationListForGettingDto>> GetReservationsOfHotelAsync(int hotelId, PagedRequestDto parameters);
        Task<PagedResponseDto<ReservationListForGettingDto>> GetReservationsBySearchParamsAsync(
            PagedRequestDto parameters,
            int? hotelId = null,
            string? guestId = null,
            int? roomId = null,
            bool? active = null,
            DateTime? date = null);
        Task<bool> HasActiveOrUpcomingReservations(string guestId);
        Task<int> UpdateReservationAsync(int reservationId, ReservationForUpdatingDto model);
        Task<int> DeleteReservationAsync(int reservationId);
    }
}
