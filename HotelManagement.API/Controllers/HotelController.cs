using HotelManagement.Application.Contracts.Service;
using HotelManagement.Application.Models.Common;
using HotelManagement.Application.Models.Hotel;
using HotelManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HotelManagement.API.Controllers
{
    [ApiController]
    [Route("api/hotel")]
    public class HotelController(IHotelService hotelService) : ControllerBase
    {

        [HttpPost("create-hotel")]
        public async Task<IActionResult> CreateNewHotel([FromBody] HotelForCreatingDto model)
        {
            var newHotel = await hotelService.CreateNewHotelAsync(model);
            var resp = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.Created),
                Result = newHotel
            };

            return StatusCode(resp.HttpStatusCode, resp);
        }

        [HttpGet("get/{hotelId}")]
        public async Task<IActionResult> GetSingleHotel([FromRoute] int hotelId)
        {
            var hotel = await hotelService.GetHotelAsync(hotelId);
            var resp = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = hotel
            };

            return StatusCode(resp.HttpStatusCode, resp);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllHotels([FromQuery] PagedRequestDto model)
        {
            var hotels = await hotelService.GetAllHotelsAsync(model);
            var resp = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = hotels
            };

            return StatusCode(resp.HttpStatusCode, resp);
        }

        [HttpGet("get-all-by-country-name/{countryName}")]
        public async Task<IActionResult> GetAllHotelsOfCountry([FromRoute] string countryName, [FromQuery] PagedRequestDto model)
        {
            var hotels = await hotelService.GetAllHotelsOfCountryAsync(countryName, model);
            var resp = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = hotels
            };

            return StatusCode(resp.HttpStatusCode, resp);
        }

        [HttpGet("get-all-by-city-name/{cityName}")]
        public async Task<IActionResult> GetAllHotelsOfCity([FromRoute] string cityName, [FromQuery] PagedRequestDto model)
        {
            var hotels = await hotelService.GetAllHotelsOfCityAsync(cityName, model);
            var resp = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = hotels
            };

            return StatusCode(resp.HttpStatusCode, resp);
        }

        [HttpGet("get-all-by-rating/{rating}")]
        public async Task<IActionResult> GetAllHotelsOfRating([FromRoute] int rating, [FromQuery] PagedRequestDto model)
        {
            var hotels = await hotelService.GetAllHotelsOfRatingAsync(rating, model);
            var resp = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = hotels
            };

            return StatusCode(resp.HttpStatusCode, resp);
        }

        [HttpPatch("update-hotel/{hotelId}")]
        public async Task<IActionResult> UpdateHotel([FromRoute] int hotelId, [FromBody] HotelForUpdatingDto model)
        {
            var res = await hotelService.UpdateHotelAsync(hotelId, model);
            var resp = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = res
            };

            return StatusCode(resp.HttpStatusCode, resp);
        }

        [HttpDelete("delete-hotel/{hotelId}")]
        public async Task<IActionResult> DeleteHotel([FromRoute] int hotelId)
        {
            var result = await hotelService.DeleteHotelAsync(hotelId);
            var resp = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = result
            };

            return StatusCode(resp.HttpStatusCode, resp);
        }
    }
}
