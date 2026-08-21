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
    [Route("api/hotels/{hotelId}/reservations")]
    public class ReservationController(IReservationService reservationService) : ControllerBase
    {
        [Authorize(Roles = "Guest")]
        [HttpPost]
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

        [HttpGet("{reservationId}")]
        public async Task<IActionResult> GetReservation([FromRoute] int hotelId, [FromRoute] int reservationId)
        {
            var reservation = await reservationService.GetReservationAsync(hotelId, reservationId);
            var resp = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = reservation
            };

            return StatusCode(resp.HttpStatusCode, resp);
        }

        [HttpGet]
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

        [HttpGet("search")]
        public async Task<IActionResult> GetReservationsBySearch(
            [FromQuery] PagedRequestDto parameters,  
            [FromQuery] int? roomId, 
            [FromQuery] int? hotelId, 
            [FromQuery] DateTime? date)
        {
            var guestId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var reservations = await reservationService.GetReservationsBySearchParamsAsync(
                                                            parameters, 
                                                            roomId, 
                                                            guestId, 
                                                            hotelId, 
                                                            date);

            var resp = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = reservations
            };

            return StatusCode(resp.HttpStatusCode, resp);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateReservation([FromRoute] int hotelId)
    }
}
