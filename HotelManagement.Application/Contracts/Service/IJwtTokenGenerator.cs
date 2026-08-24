
using HotelManagement.Domain.Entities;

namespace HotelManagement.Application.Contracts.Service
{
    public interface IJwtTokenGenerator
    {
        string GenerateAccessToken(AppUser user, IEnumerable<string> roles);
        string GenerateRefreshToken();
    }
}
