
namespace HotelManagement.Domain.Entities
{
    public class ReservationRoom
    {
        public int ReservationId { get; set; }
        public int RoomId { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<Room> Rooms { get; set; }
    }
}
