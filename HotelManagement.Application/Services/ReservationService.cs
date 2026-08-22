using HotelManagement.Application.Contracts.Persistence;
using HotelManagement.Application.Contracts.Service;
using HotelManagement.Application.Exceptions;
using HotelManagement.Application.Models.Common;
using HotelManagement.Application.Models.Reservation;
using HotelManagement.Application.Models.Room;
using HotelManagement.Domain.Entities;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HotelManagement.Application.Services
{
    public class ReservationService(IReservationRepository reservationRepo, IMapper mapper, IRoomService roomService, IReservationRoomRepository reservationRoomRepo) : IReservationService
    {
        public async Task<int> CreateReservationAsync(int hotelId, ReservationForCreatingDto model, string guestId)
        {
            if (hotelId <= 0) throw new BadRequestException("HotelId cannot be less than or equal to 0");
            if (model is null) throw new BadRequestException("Request model cannot be null");
            if (model.CheckInDate >= model.CheckOutDate) throw new BadRequestException("Check-in date must be before check-out date");
            if (model.RoomId <= 0) throw new BadRequestException("RoomId cannot be less than or equal to 0");

            var room = await roomService.GetRoomAsync(hotelId, model.RoomId);

            if (room is null) throw new NotFoundException($"Room with the Id {model.RoomId} not found");

            var conflictingReservations = await reservationRepo.GetAsync(
                filter: rs =>
                rs.ReservationRooms.Any(rr => rr.RoomId == model.RoomId) &&
                rs.CheckInDate < model.CheckOutDate &&
                rs.CheckOutDate > model.CheckInDate);

            if (conflictingReservations is not null) throw new NotAllowedException("The room is already reserved for the requested dates");

            var reservation = mapper.Map<Reservation>(model);

            reservation.GuestId = guestId;

            await reservationRepo.AddAsync(reservation);

            await reservationRepo.SaveAsync();

            var reservationRoom = new ReservationRoom
            {
                RoomId = model.RoomId,
                ReservationId = reservation.Id,
            };

            await reservationRoomRepo.AddAsync(reservationRoom);
            return await reservationRoomRepo.SaveAsync();
        }

        public async Task<ReservationForGettingDto> GetReservationAsync(int reservationId)
        {
            if (reservationId <= 0) throw new BadRequestException("Reservation Id cannot be less than or equal to 0");

            var reservation = await reservationRepo.GetAsync(
                filter: rs => rs.Id == reservationId);

            if (reservation is null) throw new NotFoundException($"Reservation with the Id {reservationId} not found");

            return mapper.Map<ReservationForGettingDto>(reservation);
        }

        public async Task<PagedResponseDto<ReservationListForGettingDto>> GetAllReservationsAsync(PagedRequestDto parameters)
        {
            var reservations = await reservationRepo.GetAllAsync(
                orderBy: BuildOrderBy(parameters.SortBy),
                ascending: parameters.Ascending,
                pageSize: parameters.PageSize,
                pageNumber: parameters.PageNumber);

            return MapToPagedResponse(reservations, parameters);
        }

        public async Task<PagedResponseDto<ReservationListForGettingDto>> GetReservationsOfHotelAsync(int hotelId, PagedRequestDto parameters)
        {
            if (hotelId <= 0) throw new BadRequestException("HotelId cannot be less than or equal to 0");

            var reservations = await reservationRepo.GetAllAsync(
                filter: rs => rs.ReservationRooms
                .Any(rr => rr.Room.HotelId == hotelId),
                orderBy: BuildOrderBy(parameters.SortBy),
                ascending: parameters.Ascending,
                pageNumber: parameters.PageNumber,
                pageSize: parameters.PageSize);

            return MapToPagedResponse(reservations, parameters);
        }

        public async Task<PagedResponseDto<ReservationListForGettingDto>> GetReservationsBySearchParamsAsync(
            PagedRequestDto parameters,
            int? hotelId = null,
            string? guestId = null,
            int? roomId = null,
            bool? active = null,
            DateTime? date = null)
        {
            var reservations = await reservationRepo.GetAllAsync(
                filter: rs =>
                    (!hotelId.HasValue ||
                        rs.ReservationRooms.Any(rr => rr.Room.HotelId == hotelId.Value)) &&
                    (guestId == null ||
                        rs.GuestId == guestId) &&
                    (!roomId.HasValue ||
                        rs.ReservationRooms.Any(rr => rr.RoomId == roomId.Value)) &&
                    (!active.HasValue ||
                        (active.Value && rs.CheckOutDate > DateTime.UtcNow) ||
                        (!active.Value && rs.CheckOutDate <= DateTime.UtcNow)) &&
                    (!date.HasValue ||
                        (rs.CheckInDate <= date.Value &&
                        rs.CheckOutDate > date.Value)),
                orderBy: BuildOrderBy(parameters.SortBy),
                ascending: parameters.Ascending,
                pageSize: parameters.PageSize,
                pageNumber: parameters.PageNumber);

            return MapToPagedResponse(reservations, parameters);
        }

        public async Task<bool> HasActiveOrUpcomingReservations(string guestId)
        {
            if (guestId is null) throw new BadRequestException("GuestId cannot be null");

            return await reservationRepo.ExistsAsync(r =>
                r.GuestId == guestId &&
                r.CheckOutDate > DateTime.UtcNow);
        }

        public async Task<int> UpdateReservationAsync(int reservationId, ReservationForUpdatingDto model)
        {
            if (reservationId <= 0) throw new BadRequestException("ReservationId cannot be less than or equal to 0");
            if (model is null) throw new BadRequestException("Request model cannot be null");

            var reservation = await reservationRepo.GetAsync(
                filter: rs => rs.Id == reservationId);

            if (reservation is null) throw new NotFoundException($"Reservation with the Id {reservation.Id} not found");

            mapper.Map(model, reservation);
            reservationRepo.Update(reservation);
            return await reservationRepo.SaveAsync();
        }

        public async Task<int> DeleteReservationAsync(int reservationId)
        {
            if (reservationId <= 0) throw new BadRequestException("ReservationId cannot be less than or equal to 0");

            var reservation = await reservationRepo.GetAsync(
                filter: rs => rs.Id == reservationId);

            if (reservation is null) throw new NotFoundException($"Reservation with the Id {reservationId} not found");

            reservationRepo.Remove(reservation);
            return await reservationRepo.SaveAsync();
        }

        //Private Helpers
        private static Expression<Func<Reservation, object>> BuildOrderBy(string sortBy)
        {
            return sortBy?.ToLower() switch
            {
                "checjin" => rs => rs.CheckInDate,
                "checkout" => rs => rs.CheckOutDate,
                _ => r => r.Id
            };
        }

        private PagedResponseDto<ReservationListForGettingDto> MapToPagedResponse(
            (IEnumerable<Reservation> Items, int TotalCount) reservations,
            PagedRequestDto parameters)
        {
            return new PagedResponseDto<ReservationListForGettingDto>
            {
                Items = reservations.Items.Any()
                    ? mapper.Map<IEnumerable<ReservationListForGettingDto>>(reservations.Items)
                    : Enumerable.Empty<ReservationListForGettingDto>(),
                TotalCount = reservations.TotalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }
    }
}
