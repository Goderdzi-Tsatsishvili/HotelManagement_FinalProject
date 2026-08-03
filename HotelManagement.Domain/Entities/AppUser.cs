
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagement.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PersonalNumber { get; set; }

        [ForeignKey(nameof(ManagerHotel))]
        public int? HotelId { get; set; } = null;

        //Navigation Properties
        public Hotel ManagerHotel { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
