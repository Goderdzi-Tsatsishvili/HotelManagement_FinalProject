
namespace HotelManagement.Application.Models.Reservation
{
    public class ReservationListForGettingDto
    {
        public int Id { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string GuestId { get; set; }
    }
}
