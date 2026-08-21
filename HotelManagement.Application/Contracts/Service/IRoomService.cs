
using HotelManagement.Application.Models.Common;
using HotelManagement.Application.Models.Room;

namespace HotelManagement.Application.Contracts.Service
{
    public interface IRoomService
    {
        Task<int> CreateNewRoomAsync(int hotelId, RoomForCreatingDto model);
        Task<RoomForGettingDto> GetRoomAsync(int hotelId, int roomId);
        Task<PagedResponseDto<RoomListForGettingDto>> GetAllRoomsAsync(int hotelId, PagedRequestDto parameters);    
        Task<PagedResponseDto<RoomListForGettingDto>> GetRoomsByPriceAndAvailabilityAsync(
            int hotelId,
            PagedRequestDto parameters,
            decimal? minprice,
            decimal? maxprice,
            DateTime date);
        Task<int> UpdateRoomAsync(int hoteId, int roomId, RoomForUpdatingDto model);
        Task<int> DeleteRoomAsync(int hotelId, int roomId);
    }
}
