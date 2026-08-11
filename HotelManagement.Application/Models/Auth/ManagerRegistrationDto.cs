
namespace HotelManagement.Application.Models.Auth
{
    public class ManagerRegistrationDto : IRegistrationDto
    {
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PersonalNumber { get; set; }
        public string PhoneNumber { get; set; }
        public int? HotelId { get; set; }
    }
}
