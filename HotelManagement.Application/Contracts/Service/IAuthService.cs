
using HotelManagement.Application.Models.Auth;

namespace HotelManagement.Application.Contracts.Service
{
    public interface IAuthService
    {
        Task<string> RegisterGuestAsync(RegistrationRequestDto request, string accountConfirmationUrl = null);
        Task<string> RegisterManagerAsync(RegistrationRequestDto request, string accountConfirmationUrl = null);
        Task<string> RegisterAdminAsync(RegistrationRequestDto request, string accountConfirmationUrl = null);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
        Task ConfirmEmailAsync(string userId, string token);
    }
}
