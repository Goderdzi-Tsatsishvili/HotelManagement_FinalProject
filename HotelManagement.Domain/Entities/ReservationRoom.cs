
namespace HotelManagement.Domain.Entities
{
    public class ReservationRoom
    {
        public int ReservationId { get; set; }
        public int RoomId { get; set; }

        public Reservation Reservation { get; set; }
        public Room Room { get; set; }
    }
}
