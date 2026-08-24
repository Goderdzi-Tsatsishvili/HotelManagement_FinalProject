
using HotelManagement.Application.Models.Manager;

namespace HotelManagement.Application.Models.Hotel
{
    public class HotelWithManagerForGettingDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public ICollection<ManagerForGettingDto> Managers { get; set; }
    }
}
