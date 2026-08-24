
namespace HotelManagement.Application.Models.Notification
{
    public record EmailSendResponse(bool success, string message, Exception error = null);
}
