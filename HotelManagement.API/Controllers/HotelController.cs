using HotelManagement.Application.Contracts.Service;
using HotelManagement.Application.Models.Auth;
using HotelManagement.Application.Models.Common;
using HotelManagement.Application.Models.Hotel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HotelManagement.API.Controllers
{
    [ApiController]
    [Route("api/hotels")]
    public class HotelController(IHotelService hotelService, IAuthService authService) : ControllerBase
    {
        [Authorize(Roles = "Admon")]
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

        [HttpGet("search")]
        public async Task<IActionResult> GetHotelsBySearch([FromQuery] string? countryName, [FromQuery] string? city, [FromQuery] int? rating, [FromQuery] PagedRequestDto model)
        {
            var hotels = await hotelService.GetAllHotelsBySearchParamsAsync(countryName, city, rating, model);
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

        [Authorize(Roles = "Admin")]
        [HttpPost("{hotelId}/managers")]
        public async Task<IActionResult> AddManager([FromRoute] int hotelId, [FromBody] ManagerRegistrationDto model)
        {
            var confirmationBaseUrl = BuildConfirmationBaseUrl(Request);
            var res = await authService.RegisterManagerAsync(hotelId, model, confirmationBaseUrl);
            var resp = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = res
            };

            return StatusCode(resp.HttpStatusCode, resp);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("managers/{managerId}/delete-manager")]
        public async Task<IActionResult> DeleteManager([FromRoute] string managerId)
        {
            var res = await authService.DeleteManagerAsync(managerId);
            var resp = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = res
            };

            return StatusCode(resp.HttpStatusCode, resp);
        }

        //Private Helper
        private static string BuildConfirmationBaseUrl(HttpRequest request)
        {
            return $"{request.Scheme}://{request.Host}/api/auth/confirm-email";
        }
    }
}
