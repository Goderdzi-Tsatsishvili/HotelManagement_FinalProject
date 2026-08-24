
namespace HotelManagement.Application.Models.Auth
{
    public record LoginResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
