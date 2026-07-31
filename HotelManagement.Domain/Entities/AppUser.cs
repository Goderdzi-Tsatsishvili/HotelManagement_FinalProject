
using Microsoft.AspNetCore.Identity;

namespace HotelManagement.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public int FirstName { get; set; }
        //Navigation Properties
        public ICollection<Reservation> Reservations { get; set; }
    }
}
