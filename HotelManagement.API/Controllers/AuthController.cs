using HotelManagement.Application.Contracts.Service;
using HotelManagement.Application.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace HotelManagement.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] AdminRegistrationDto request)
        {
            var confirmationBaseUrl = BuildConfirmationBaseUrl(Request);
            var res = await authService.RegisterAdminAsync(request, confirmationBaseUrl);
            var resp = new CommonResponse()
            {
                Message = "Admin Registered Successfully",
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.Created),
                Result = res
            };

            return StatusCode(resp.HttpStatusCode, resp);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterGuest([FromBody] GuestRegistrationDto request)
        {
            var confirmationBaseUrl = BuildConfirmationBaseUrl(Request);
            var res = await authService.RegisterGuestAsync(request, confirmationBaseUrl);
            var resp = new CommonResponse()
            {
                Message = "Guest Registered Successfully",
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.Created),
                Result = res
            };

            return StatusCode(resp.HttpStatusCode, resp);
        }

        [HttpPatch("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginRequestDto request)
        {
            var res = await authService.LoginAsync(request);
            var resp = new CommonResponse()
            {
                Message = "User Logged-in Successfully",
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = res
            };

            return StatusCode(resp.HttpStatusCode, resp);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            await authService.ConfirmEmailAsync(userId, token);
            var resp = new CommonResponse()
            {
                Message = "Email Confirmed Successfully",
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK)
            };

            return StatusCode(resp.HttpStatusCode, resp);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("guest/delete-guest")]
        public async Task<IActionResult> DeleteGuest(string guestId)
        {
            var res = await authService.DeleteGuestAsync(guestId);
            var resp = new CommonResponse()
            {
                Message = CommonResponseMessage.SuccessMessage,
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = res
            };

            return StatusCode(resp.HttpStatusCode, resp);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] string newPassword)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var res = authService.ResetPasswordAsync(userId, newPassword);
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
