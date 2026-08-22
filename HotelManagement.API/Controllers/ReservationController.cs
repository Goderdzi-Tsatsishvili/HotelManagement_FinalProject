using HotelManagement.Application.Contracts.Service;
using HotelManagement.Application.Models.Common;
using HotelManagement.Application.Models.Reservation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace HotelManagement.API.Controllers
{
    [ApiController]
    [Route("api/hotels")]
    public class ReservationController(IReservationService reservationService) : ControllerBase
    {
        [Authorize(Roles = "Guest")]
        [HttpPost("{hotelId}/reservations")]
        public async Task<IActionResult> CreateReservation([FromRoute] int hotelId, [FromBody] ReservationForCreatingDto model)
        {
            var guestId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var newReservation = await reservationService.CreateReservationAsync(hotelId, model, guestId);
            var resp = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.Created),
                Result = newReservation
            };

            return StatusCode(resp.HttpStatusCode, resp);
        }

        [HttpGet("reservations/{reservationId}")]
        public async Task<IActionResult> GetReservation([FromRoute] int reservationId)
        {
            var reservation = await reservationService.GetReservationAsync(reservationId);
            var resp = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = reservation
            };

            return StatusCode(resp.HttpStatusCode, resp);
        }

        [HttpGet("{hotelId}/reservations/get")]
        public async Task<IActionResult> GetReservationsOfHotel([FromRoute] int hotelId, [FromQuery] PagedRequestDto parameters)
        {
            var reservations = await reservationService.GetReservationsOfHotelAsync(hotelId, parameters);
            var resp = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = reservations
            };

            return StatusCode(resp.HttpStatusCode, resp);
        }

        [Authorize(Roles = "Guest")]
        [HttpGet("reservations/search")]
        public async Task<IActionResult> GetReservationsBySearch(
            [FromQuery] PagedRequestDto parameters,  
            [FromQuery] int? roomId, 
            [FromQuery] int? hotelId,
            [FromQuery] bool? active,
            [FromQuery] DateTime? date)
        {
            var guestId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var reservations = await reservationService.GetReservationsBySearchParamsAsync(parameters, roomId, guestId, hotelId, active, date);

            var resp = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = reservations
            };

            return StatusCode(resp.HttpStatusCode, resp);
        }

        [HttpGet("reservations/get-all-of-guest")]
        public async Task<IActionResult> GetAllReservationsOfGuest(PagedRequestDto parameters)
        {
            var reservations = await reservationService.GetAllReservationsAsync(parameters);
            var resp = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = reservations
            };

            return StatusCode(resp.HttpStatusCode, resp);
        }

        [HttpPatch("reservations/{reservationId}/update")]
        public async Task<IActionResult> UpdateReservation([FromRoute] int reservationId, [FromBody] ReservationForUpdatingDto model)
        {
            var res = await reservationService.UpdateReservationAsync(reservationId, model);
            var resp = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = res
            };

            return StatusCode(resp.HttpStatusCode, resp);
        }

        [HttpDelete("reservations/{reservationId}/remove")]
        public async Task<IActionResult> DeleteReservation([FromRoute] int reservationId)
        {
            var res = await reservationService.DeleteReservationAsync(reservationId);
            var resp = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = res
            };

            return StatusCode(resp.HttpStatusCode, resp);
        }
    }
}
