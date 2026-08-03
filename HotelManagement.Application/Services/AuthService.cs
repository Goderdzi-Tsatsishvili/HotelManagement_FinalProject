
using HotelManagement.Application.Contracts.Persistence;
using HotelManagement.Application.Contracts.Service;
using HotelManagement.Application.Exceptions;
using HotelManagement.Application.Models.Auth;
using HotelManagement.Domain.Entities;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace HotelManagement.Application.Services
{
    public class AuthService(
        IRefreshTokenRepository refreshTokenRepo,
        IJwtTokenGenerator jwtGenerator,
        IConfiguration config,
        IEmailService emailService,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IMapper mapper
        ) : IAuthService
    {

        private const string _adminRole = "Admin";
        private const string _managerRole = "Manager";
        private const string _guestRole = "Guest";
        private const string _confirmEmailTitle = "Confirm Email";

        public async Task<string> RegisterGuestAsync(RegistrationRequestDto request, string accountConfirmationUrl = null)
        {
            return await RegisterUserAsync(
                request,
                _guestRole,
                accountConfirmationUrl);
        }

        public async Task<string> RegisterManagerAsync(RegistrationRequestDto request, string accountConfirmationUrl = null)
        {
            return await RegisterUserAsync(
                request,
                _managerRole,
                accountConfirmationUrl);
        }

        public async Task<string> RegisterAdminAsync(RegistrationRequestDto request, string accountConfirmationUrl = null)
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

        private async Task<string> RegisterUserAsync(
            RegistrationRequestDto registrationRequestDto,
            string role,
            string accountConfirmationUrl = null)
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
