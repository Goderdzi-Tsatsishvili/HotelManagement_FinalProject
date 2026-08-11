
using HotelManagement.Application.Models.Auth;

namespace HotelManagement.Application.Contracts.Service
{
    public interface IAuthService
    {
        Task<string> RegisterGuestAsync(GuestRegistrationDto request, string accountConfirmationUrl = null);
        Task<string> RegisterManagerAsync(ManagerRegistrationDto request, string accountConfirmationUrl = null);
        Task<string> RegisterAdminAsync(AdminRegistrationDto request, string accountConfirmationUrl = null);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
        Task ConfirmEmailAsync(string userId, string token);
    }
}
