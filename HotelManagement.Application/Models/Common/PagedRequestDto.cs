
namespace HotelManagement.Application.Models.Common
{
    public record PagedRequestDto
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string SortBy { get; set; }
        public bool Ascending { get; set; }
    }
}
