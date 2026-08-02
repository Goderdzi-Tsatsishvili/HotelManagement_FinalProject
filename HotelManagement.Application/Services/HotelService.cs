
using HotelManagement.Application.Contracts.Persistence;
using HotelManagement.Application.Contracts.Service;
using HotelManagement.Application.Exceptions;
using HotelManagement.Application.Models.Hotel;
using HotelManagement.Domain.Entities;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Application.Services
{
    public class HotelService(IHotelRepository hotelRepo, IMapper mapper) : IHotelService
    {
        public async Task<int> CreateNewHotelAsync(HotelForCreatingDto model)
        {
            if (model is null) throw new BadRequestException("Creation Model cannot be empty");

            var newHotel = mapper.Map<Hotel>(model);
            await hotelRepo.AddAsync(newHotel);
            return await hotelRepo.SaveAsync();
        }

        public async Task<HotelForGettingDto> GetHotelAsync(int hotelId)
        {
            if (hotelId <= 0) throw new BadRequestException("HotelId cannot be less or equal to 0");

            var hotel = await hotelRepo.GetAsync(
                filter: h => h.Id == hotelId);

            if (hotel is null) throw new NotFoundException($"Hotel with the Id {hotelId} wasnt found");
            return mapper.Map<HotelForGettingDto>(hotel);
        }

        public async Task<int> DeleteHotelAsync(int hotelId)
        {
            if (hotelId <= 0) throw new BadRequestException("HotelId cannot be less or equal to 0");

            var hotel = await hotelRepo.GetAsync(
                filter: h => h.Id == hotelId,
                include: h => h
                    .Include(h => h.Rooms)
                        .ThenInclude(r => r.ReservationRooms)
                            .ThenInclude(rr => rr.Reservation));

            if (hotel is null) throw new NotFoundException($"Hotel With the Id {hotelId} not found");

            if (hotel.Rooms.Count != 0) throw new NotAllowedException("Hotel Deletion Not Allowed Because the Rooms arent Empty");
            if (hotel.Rooms
                .SelectMany(r => r.ReservationRooms)
                .Any(rr => rr.Reservation.CheckOutDate > DateTime.UtcNow))
                throw new NotAllowedException("Hotel Cannot have any active reservations");

            hotelRepo.Remove(hotel);
            return await hotelRepo.SaveAsync();
        }
    }
}
