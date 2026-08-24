
namespace HotelManagement.Application.Models.Room
{
    public class RoomForGettingDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int HotelId { get; set; }
    }
}
