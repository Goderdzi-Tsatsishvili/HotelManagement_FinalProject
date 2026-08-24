using HotelManagement.Application.Contracts.Service;
using HotelManagement.Application.Models.Common;
using HotelManagement.Application.Models.Room;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HotelManagement.API.Controllers
{
    [ApiController]
    [Route("api/hotels")]
    public class RoomController(IRoomService roomService) : ControllerBase
    {
        [HttpGet("{hotelId}/rooms/{roomId}")]
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

        [HttpGet("{hotelId}/rooms/get-by-price")]
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

        [Authorize(Roles = "Admin")]
        [HttpGet("rooms/get-all")]
        public async Task<IActionResult> GetRooms([FromQuery] PagedRequestDto parameters)
        {
            var rooms = await roomService.GetAllRoomsAsync(parameters);
            var resp = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = rooms
            };

            return StatusCode(resp.HttpStatusCode, resp);
        }


        [HttpGet("{hotelId}/rooms/get-all-of-hotel")]
        public async Task<IActionResult> GetAllRoomsOfHotel([FromRoute] int hotelId, [FromQuery] PagedRequestDto parameters)
        {
            var rooms = await roomService.GetAllRoomsOfHotelAsync(hotelId, parameters);
            var resp = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = rooms
            };

            return StatusCode(resp.HttpStatusCode, resp);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost("{hotelId}/rooms/create-new-room")]
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

        [Authorize(Roles = "Manager")]
        [HttpPatch("{hotelId}/rooms/{roomId}/update")]
        public async Task<IActionResult> UpdateRoom([FromRoute] int hotelId, [FromRoute] int roomId, [FromBody] RoomForUpdatingDto model)
        {
            var res = await roomService.UpdateRoomAsync(hotelId, roomId, model);
            var resp = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = res
            };

            return StatusCode(resp.HttpStatusCode, resp);
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete("{hotelId}/rooms/{roomId}/remove")]
        public async Task<IActionResult> DeleteRoom([FromRoute] int hotelId, [FromRoute] int roomId)
        {
            var res = await roomService.DeleteRoomAsync(hotelId, roomId);
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
