
using HotelManagement.Application.Contracts.Persistence;
using HotelManagement.Application.Contracts.Service;
using HotelManagement.Application.Exceptions;
using HotelManagement.Application.Models.Common;
using HotelManagement.Application.Models.Room;
using HotelManagement.Domain.Entities;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HotelManagement.Application.Services
{
    public class RoomService(IRoomRepository roomRepo, IMapper mapper) : IRoomService
    {
        public async Task<int> CreateNewRoomAsync(int hotelId, RoomForCreatingDto model)
        {
            if (model is null) throw new BadRequestException("Request model cannot be null");
            if (model.Price <= 0) throw new NotAllowedException("The room price cannot be less than or equal to 0");

            var newRoom = mapper.Map<Room>(model);
            newRoom.HotelId = hotelId;
            await roomRepo.AddAsync(newRoom);
            return await roomRepo.SaveAsync();
        }

        public async Task<RoomForGettingDto> GetRoomAsync(int hotelId, int roomId)
        {
            if (roomId <= 0) throw new BadRequestException("Request Id cannot be less or equal to 0");

            var room = await roomRepo.GetAsync(
                filter: r => r.Id == roomId
                &&
                r.HotelId == hotelId);

            if (room is null) throw new NotFoundException($"Room with the Id {roomId} not found");
            return mapper.Map<RoomForGettingDto>(room);
        }

        public async Task<PagedResponseDto<RoomListForGettingDto>> GetAllRoomsAsync(int hotelId, PagedRequestDto parameters)
        {
            var rooms = await roomRepo.GetAllAsync(
                filter: r => r.HotelId == hotelId,
                orderBy: BuildOrderBy(parameters.SortBy),
                ascending: parameters.Ascending,
                pageNumber: parameters.PageNumber,
                pageSize: parameters.PageSize);

            return MapToPagedResponse(rooms, parameters);
        }

        public async Task<PagedResponseDto<RoomListForGettingDto>> GetRoomsByPriceAndAvailabilityAsync(
            int hotelId,
            PagedRequestDto parameters,
            decimal? minprice,
            decimal? maxprice,
            DateTime date)
        {
            if (hotelId <= 0) throw new BadRequestException("HotelId cannot be less than or equal to 0");
            if (minprice <= 0) throw new BadRequestException("MinPrice cannot be less than or equal to 0");
            if (maxprice <= 0) throw new BadRequestException("MaxPrice cannot be less than or eqaul to 0");

            var rooms = await roomRepo.GetAllAsync(
                filter: r => r.HotelId == hotelId &&
                             (!minprice.HasValue || r.Price >= minprice.Value) &&
                             (!maxprice.HasValue || r.Price <= maxprice.Value) &&
                             !r.ReservationRooms.Any(rr => rr.Reservation.CheckInDate <= date &&
                                                     rr.Reservation.CheckOutDate > date),
                orderBy: BuildOrderBy(parameters.SortBy),
                ascending: parameters.Ascending,
                pageNumber: parameters.PageNumber,
                pageSize: parameters.PageSize);

            return MapToPagedResponse(rooms, parameters);
        }

        public async Task<int> UpdateRoomAsync(int hotelId, int roomId, RoomForUpdatingDto model)
        {
            if (model is null) throw new BadRequestException("Request model cannot be null");
            if (roomId <= 0) throw new BadRequestException("Room Id cannot be less than or equal to 0");
            if (hotelId <= 0) throw new BadRequestException("Hotel Id cannot be less than or equal to 0");
            if (model.Price <= 0) throw new NotAllowedException("The room price cannot be less than or equal to 0");

            var room = await roomRepo.GetAsync(
                filter: r => r.Id == roomId && r.HotelId == hotelId);

            if (room is null) throw new NotFoundException($"Room with the Id {roomId} not found");

            mapper.Map(model, room);
            roomRepo.Update(room);
            return await roomRepo.SaveAsync();
        }

        public async Task<int> DeleteRoomAsync(int hotelId, int roomId)
        {
            if (hotelId <= 0) throw new BadRequestException("Hotel Id cannot be less than or equal to 0");
            if (roomId <= 0) throw new BadRequestException("Room Id cannot be less than or equal to 0");

            var room = await roomRepo.GetAsync(
                filter: r => r.Id == roomId && r.HotelId == hotelId,
                include: r => r
                .Include(r => r.ReservationRooms)
                    .ThenInclude(rr => rr.Reservation));

            if (room is null) throw new NotFoundException($"Room with the Id {roomId} not found");
            if (room.ReservationRooms
                .Select(rs => rs.Reservation)
                .Any(rs => rs.CheckInDate <= DateTime.UtcNow && rs.CheckOutDate > DateTime.UtcNow))
                throw new NotAllowedException("Room cannot be deleted bacause it has an active reservation");
            if (room.ReservationRooms
                .Select(rs => rs.Reservation)
                .Any(rs => rs.CheckInDate > DateTime.UtcNow && rs.CheckOutDate > DateTime.UtcNow))
                throw new NotAllowedException("Room cannot be deleted because it has an upcoming reservation");

            roomRepo.Remove(room);
            return await roomRepo.SaveAsync();
        }

        //Private Helpers
        private static Expression<Func<Room, object>> BuildOrderBy(string sortBy)
        {
            return sortBy?.ToLower() switch
            {
                "name" => r => r.Name,
                "price" => r => r.Price,
                _ => r => r.Id
            };
        }

        private PagedResponseDto<RoomListForGettingDto> MapToPagedResponse(
            (IEnumerable<Room> Items, int TotalCount) rooms,
            PagedRequestDto parameters)
        {
            return new PagedResponseDto<RoomListForGettingDto>
            {
                Items = rooms.Items.Any()
                    ? mapper.Map<IEnumerable<RoomListForGettingDto>>(rooms.Items)
                    : Enumerable.Empty<RoomListForGettingDto>(),
                TotalCount = rooms.TotalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }
    }
}
