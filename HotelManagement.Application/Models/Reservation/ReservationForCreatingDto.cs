
namespace HotelManagement.Application.Models.Reservation
{
    public class ReservationForCreatingDto
    {
        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
