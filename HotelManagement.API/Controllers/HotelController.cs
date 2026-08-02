using HotelManagement.Application.Contracts.Service;
using HotelManagement.Application.Models.Hotel;
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
        public async Task<IActionResult> GetSingleHotel(int hotelId)
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
    }
}
