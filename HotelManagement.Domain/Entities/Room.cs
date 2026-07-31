
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelManagement.Domain.Entities
{
    public class Room
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        [ForeignKey(nameof(RoomHotel))]
        public int HotelId { get; set; }

        //Navigation Properties
        public Hotel RoomHotel { get; set; }
    }
}
