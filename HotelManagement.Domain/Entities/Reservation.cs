
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.PortableExecutable;

namespace HotelManagement.Domain.Entities
{
    public class Reservation
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        [ForeignKey(nameof(Guest))]
        public string GuestId { get; set; }


        //Navigation Propreties
        public AppUser Guest { get; set; }
        public ICollection<ReservationRoom> ReservationRooms { get; set; }
    }
}
