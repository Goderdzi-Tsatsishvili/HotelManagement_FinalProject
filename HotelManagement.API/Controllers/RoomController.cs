using HotelManagement.Application.Contracts.Service;
using HotelManagement.Application.Models.Common;
using HotelManagement.Application.Models.Room;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HotelManagement.API.Controllers
{
    [ApiController]
    [Route("api/hotels/{hotelId}/rooms")]
    public class RoomController(IRoomService roomService) : ControllerBase
    {
        [HttpGet("{roomId}")]
        public async Task<IActionResult> GetRoom([FromRoute] int hotelId, [FromRoute] int roomId)
        {
            var room = await roomService.GetRoomAsync(hotelId, roomId);
            var resp = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = room
            };

            return StatusCode(resp.HttpStatusCode, resp);
        }

        [HttpGet("get-by-price")]
        public async Task<IActionResult> GetRoomByPriceRange(
            [FromRoute] int hotelId, 
            [FromQuery] PagedRequestDto parameters, 
            [FromQuery] decimal minPrice, 
            [FromQuery] decimal maxPrice, 
            [FromQuery] DateTime date)
        {
            var res = await roomService.GetRoomsByPriceAndAvailabilityAsync(hotelId, parameters, minPrice, maxPrice, date);
            var resp = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = res
            };

            return StatusCode(resp.HttpStatusCode, resp);
        }

        [HttpGet]
        public async Task<IActionResult> GetRooms([FromRoute] int hotelId, [FromQuery] PagedRequestDto parameters)
        {
            var rooms = await roomService.GetAllRoomsAsync(hotelId, parameters);
            var resp = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = rooms
            };

            return StatusCode(resp.HttpStatusCode, resp);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromRoute] int hotelId, [FromBody] RoomForCreatingDto model)
        {
            var res = await roomService.CreateNewRoomAsync(hotelId, model);
            var resp = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.Created),
                Result = res
            };

            return StatusCode(resp.HttpStatusCode, resp);
        }
    }
}
