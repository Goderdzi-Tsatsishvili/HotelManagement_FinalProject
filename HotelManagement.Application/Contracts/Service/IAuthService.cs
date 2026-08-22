
using HotelManagement.Application.Models.Auth;

namespace HotelManagement.Application.Contracts.Service
{
    public interface IAuthService
    {
        Task<string> RegisterGuestAsync(GuestRegistrationDto request, string accountConfirmationUrl = null);
        Task<string> RegisterManagerAsync(int hotelId, ManagerRegistrationDto request, string accountConfirmationUrl = null);
        Task<string> RegisterAdminAsync(AdminRegistrationDto request, string accountConfirmationUrl = null);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
        Task ConfirmEmailAsync(string userId, string token);
        Task ResetPasswordAsync(string userId, string newPassword);
        Task<int> DeleteManagerAsync(string managerId);
        Task<int> DeleteGuestAsync(string guestId);
    }
}
