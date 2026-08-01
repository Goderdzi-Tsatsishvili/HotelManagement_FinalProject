
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagement.Domain.Entities
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }
        public DateTime CheckInDate { get; set; }

        [ForeignKey(nameof(Guest))]
        public string GuestId { get; set; }


        //Navigation Propreties
        public AppUser Guest { get; set; }
        public ICollection<ReservationRoom> ReservationRooms { get; set; }
    }
}
