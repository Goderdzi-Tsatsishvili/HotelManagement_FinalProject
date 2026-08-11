
using HotelManagement.Application.Models.Common;
using HotelManagement.Application.Models.Room;

namespace HotelManagement.Application.Contracts.Service
{
    public interface IRoomService
    {
        Task<int> CreateNewRoomAsync(RoomForCreatingDto model);
        Task<RoomForGettingDto> GetRoomAsync(int roomId);
        Task<PagedResponseDto<RoomListForGettingDto>> GetAllRoomsAsync(PagedRequestDto parameters);
        Task<PagedResponseDto<RoomListForGettingDto>> GetRoomsByPriceAndAvailabilityAsync(
            PagedRequestDto parameters,
            decimal? minprice,
            decimal? maxprice,
            DateTime date);
        Task<int> UpdateRoomAsync(int roomId, RoomForUpdatingDto model);
        Task<int> DeleteRoomAsync(int roomId);
    }
}
