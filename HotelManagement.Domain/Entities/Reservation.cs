
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagement.Domain.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime CheckInDate { get; set; }

        [ForeignKey(nameof(Guest))]
        public int GuestId { get; set; }


        //Navigation Propreties
        public AppUser Guest { get; set; }
    }
}
