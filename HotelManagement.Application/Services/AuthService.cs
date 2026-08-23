
using HotelManagement.Application.Contracts.Persistence;
using HotelManagement.Application.Contracts.Service;
using HotelManagement.Application.Exceptions;
using HotelManagement.Application.Models.Auth;
using HotelManagement.Application.Models.Common;
using HotelManagement.Domain.Entities;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HotelManagement.Application.Services
{
    public class AuthService(
        IRefreshTokenRepository refreshTokenRepo,
        IJwtTokenGenerator jwtGenerator,
        IConfiguration config,
        IEmailService emailService,
        IHotelService hotelService,
        IReservationService reservationService,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IMapper mapper
        ) : IAuthService
    {

        private const string _adminRole = "Admin";
        private const string _managerRole = "Manager";
        private const string _guestRole = "Guest";
        private const string _confirmEmailTitle = "Confirm Email";

        public async Task<string> RegisterGuestAsync(GuestRegistrationDto request, string accountConfirmationUrl = null)
        {
            return await RegisterUserAsync(
                request,
                _guestRole,
                accountConfirmationUrl);
        }

        public async Task<string> RegisterManagerAsync(int hotelId, ManagerRegistrationDto request, string accountConfirmationUrl = null)
        {
            var hotel = await hotelService.GetHotelAsync(hotelId);
            
            var userId = await RegisterUserAsync(
                request,
                _managerRole,
                accountConfirmationUrl);

            var manager = await userManager.FindByIdAsync(userId);

            if(manager is null) throw new NotFoundException($"Manager '{manager.FirstName} {manager.LastName}' not found");

            manager.HotelId = hotelId;
            await userManager.UpdateAsync(manager);
            return userId;
        }

        public async Task<string> RegisterAdminAsync(AdminRegistrationDto request, string accountConfirmationUrl = null)
        {
            return await RegisterUserAsync(
                request,
                _adminRole,
                accountConfirmationUrl);
        }

        public async Task ConfirmEmailAsync(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
                throw new BadRequestException(
                    "User id and token are required parameters for email confirmation");

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
                throw new BadRequestException("User not found");

            var result = await userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
                throw new BadRequestException(result.Errors.First().Description);

            await userManager.SetLockoutEnabledAsync(user, true);
            await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now);
            await userManager.ResetAccessFailedCountAsync(user);
        }

        public async Task ResetPasswordAsync(string userId, string newPassword)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user is null) throw new NotFoundException($"User '{user.FirstName} {user.LastName}' Not Fount");

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var result = await userManager.ResetPasswordAsync(user, token, newPassword);

            if (!result.Succeeded) throw new BadRequestException(result.Errors.First().Description);
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Email.Trim());

            if (user == null)
                throw new BadRequestException("User with provided credentials not found");

            if (!user.EmailConfirmed)
                throw new UnauthorizedException("Unable to sign in with locked account. Check your email and activate account first");

            bool isValid = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (!isValid)
                throw new BadRequestException("Username or Password is incorrect");

            var roles = await userManager.GetRolesAsync(user);

            return await GenerateTokenPairAsync(user, roles);
        }

        public async Task<int> DeleteManagerAsync(string managerId)
        {
            if (managerId is null) throw new BadRequestException("ManagerId cannot be null");

            var manager = await userManager.FindByIdAsync(managerId);

            if (manager is null) throw new NotFoundException($"Manager '{manager.FirstName} {manager.LastName}' Not Found");

            var roles = await userManager.GetRolesAsync(manager);

            if (!roles.Contains("Manager")) throw new NotAllowedException("The Specified user is not a Manager");

            var hotel = await hotelService.GetHotelWithManagerAsync(manager.HotelId);

            if (hotel is null) throw new NotFoundException($"Hotel with the Id {manager.HotelId} not found");

            if (!hotel.Managers.Any(m => m.ManagerId != manager.Id) || hotel.Managers.Count == 1) throw new NotAllowedException("The hotel must have another manager before this manager can be deleted");

            var result = await userManager.DeleteAsync(manager);

            if (!result.Succeeded) throw new BadRequestException(result.Errors.First().Description);

            return 1;
        }

        public async Task<int> DeleteGuestAsync(string guestId)
        {
            if (guestId is null) throw new BadRequestException("GuestId cannot be null");

            var guest = await userManager.FindByIdAsync(guestId);

            if (guest is null) throw new NotFoundException($"Guest '{guest.FirstName} {guest.LastName}' Not Found");

            var roles = await userManager.GetRolesAsync(guest);

            if (!roles.Contains("Guest")) throw new NotAllowedException("The specified user is not a Guest");

            var result = await reservationService.HasActiveOrUpcomingReservations(guestId);

            if (result == null) throw new NotFoundException($"The guest '{guest.FirstName} {guest.LastName}' doesnt have any reservations");

            return 1;
        }

        //Private Helpers
        private async Task<LoginResponseDto> GenerateTokenPairAsync(AppUser user, IList<string> roles)
        {
            var accessToken = jwtGenerator.GenerateAccessToken(user, roles);

            var refreshToken = new RefreshToken
            {
                Token = jwtGenerator.GenerateRefreshToken(),
                UserId = user.Id,
                CreatedAt = DateTimeOffset.Now,
                ExpiresAt = DateTimeOffset.Now.AddDays(int.Parse(config["Jwt:RefreshTokenExpiryDays"]))
            };

            await refreshTokenRepo.AddAsync(refreshToken);
            await refreshTokenRepo.SaveAsync();

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };
        }

        private static string BuildAccountConfirmationUrl(string accountConfirmationUrl, AppUser userToReturn, string token)
        {
            return $"{accountConfirmationUrl}" +
                   $"?userId={Uri.EscapeDataString(userToReturn.Id)}" +
                   $"&token={Uri.EscapeDataString(token)}";
        }
        private static string EmailConfirmationBody(string confirmationUrl)
        {
            return $@"
                <h2>Account Activation</h2>
                <p>Your administrator account has been created.</p>
                <p>Please click the link below to activate your account:</p>
                <p>
                    <a href=""{confirmationUrl}"">
                        Activate Account
                    </a>
                </p>";
        }
        private async Task EnsureRoleExistsAsync(string role)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        private async Task<string> RegisterUserAsync<TDto>(
            TDto registrationRequestDto,
            string role,
            string accountConfirmationUrl = null)
            where TDto : IRegistrationDto
        {
            var user = mapper.Map<AppUser>(registrationRequestDto);

            user.EmailConfirmed = false;
            user.LockoutEnabled = false;
            user.LockoutEnd = null;

            var result = await userManager.CreateAsync(
                user,
                registrationRequestDto.Password);

            if (!result.Succeeded)
                throw new BadRequestException(result.Errors.First().Description);

            await EnsureRoleExistsAsync(role);

            await userManager.AddToRoleAsync(user, role);

            var confirmationToken =
                await userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmationUrl = BuildAccountConfirmationUrl(
                accountConfirmationUrl,
                user,
                confirmationToken);

            var emailResponse = await emailService.Send(
                user.Email,
                _confirmEmailTitle,
                EmailConfirmationBody(confirmationUrl));

            if (!emailResponse.success)
                throw new InternalServerException(emailResponse.error.Message);

            return user.Id;
        }
    }
}
