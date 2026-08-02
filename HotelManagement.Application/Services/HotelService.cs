
using HotelManagement.Application.Contracts.Persistence;
using HotelManagement.Application.Contracts.Service;
using HotelManagement.Application.Exceptions;
using HotelManagement.Application.Models.Common;
using HotelManagement.Application.Models.Hotel;
using HotelManagement.Domain.Entities;
using MapsterMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace HotelManagement.Application.Services
{
    public class HotelService(IHotelRepository hotelRepo, IMapper mapper) : IHotelService
    {
        public async Task<int> CreateNewHotelAsync(HotelForCreatingDto model)
        {
            if (model is null) throw new BadRequestException("Creation Model cannot be empty");

            if (model.Rating > 5 || model.Rating < 0) throw new NotAllowedException("Hotel rating cannot be more than 5 or less than 0");

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

        public async Task<PagedResponseDto<HotelListForGettingDto>> GetAllHotelsAsync(PagedRequestDto parameters)
        {
            var hotels = await hotelRepo.GetAllAsync(
                orderBy: BuildOrderBy(parameters.SortBy),
                ascending: parameters.Ascending,
                pageNumber: parameters.PageNumber,
                pageSize: parameters.PageSize);

            return MapToPagedResponse(hotels, parameters);
        }

        public async Task<PagedResponseDto<HotelListForGettingDto>> GetAllHotelsOfCountryAsync(string countryName, PagedRequestDto parameters)
        {
            if (countryName is null) throw new BadRequestException("CountryName cannot be null");

            var hotels = await hotelRepo.GetAllAsync(
                filter: h => h.Country == countryName,
                orderBy: BuildOrderBy(parameters.SortBy),
                ascending: parameters.Ascending,
                pageNumber: parameters.PageNumber,
                pageSize: parameters.PageSize);

            return MapToPagedResponse(hotels, parameters);
        }

        public async Task<PagedResponseDto<HotelListForGettingDto>> GetAllHotelsOfCityAsync(string cityName, PagedRequestDto parameters)
        {
            if (cityName is null) throw new BadRequestException("CityName cannot be null");

            var hotels = await hotelRepo.GetAllAsync(
                filter: h => h.City == cityName,
                orderBy: BuildOrderBy(parameters.SortBy),
                ascending: parameters.Ascending,
                pageNumber: parameters.PageNumber,
                pageSize: parameters.PageSize);

            return MapToPagedResponse(hotels, parameters);
        }

        public async Task<PagedResponseDto<HotelListForGettingDto>> GetAllHotelsOfRatingAsync(int rating, PagedRequestDto parameters)
        {
            if (rating < 0 || rating > 5) throw new BadRequestException("Rating cannot be less than 0 or more than 5");

            var hotels = await hotelRepo.GetAllAsync(
                filter: h => h.Rating >= rating,
                orderBy: BuildOrderBy(parameters.SortBy),
                ascending: parameters.Ascending,
                pageNumber: parameters.PageNumber,
                pageSize: parameters.PageSize);

            return MapToPagedResponse(hotels, parameters);
        }

        public async Task<int> UpdateHotelAsync(HotelForUpdatingDto model)
        {
            if (model is null) throw new BadRequestException("Request model cannot be null");
            if (model.Name is null) throw new BadRequestException("Model name cannot be null");
            if (model.Address is null) throw new BadRequestException("Model address cannot be null");
            if (model.Rating < 0) throw new BadRequestException("Model rating annot be less than 0");

            var hotel = await hotelRepo.GetAsync(filter: h => h.Name == model.Name);

            if (hotel is null) throw new NotFoundException($"Hotel with the name '{model.Name} wasnt found'");

            mapper.Map(model, hotel);
            hotelRepo.Update(hotel);
            return await hotelRepo.SaveAsync();

        }

        //Private Helpers
        private static Expression<Func<Hotel, object>> BuildOrderBy(string sortBy)
        {
            return sortBy?.ToLower() switch
            {
                "country" => h => h.Country,
                "city" => h => h.City,
                "rating" => h => h.Rating,
                _ => h => h.Id
            };
        }

        private PagedResponseDto<HotelListForGettingDto> MapToPagedResponse(
            (IEnumerable<Hotel> Items, int TotalCount) hotels,
            PagedRequestDto parameters)
        {
            return new PagedResponseDto<HotelListForGettingDto>
            {
                Items = hotels.Items.Any()
                    ? mapper.Map<IEnumerable<HotelListForGettingDto>>(hotels.Items)
                    : Enumerable.Empty<HotelListForGettingDto>(),
                TotalCount = hotels.TotalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }
    }
}
