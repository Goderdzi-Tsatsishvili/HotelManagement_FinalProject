
namespace HotelManagement.Application.Models.Common
{
    public class PagedRequestDto
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string SortBy { get; set; }
        public bool Ascending { get; set; }
    }
}
