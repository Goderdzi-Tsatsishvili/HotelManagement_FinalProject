
namespace HotelManagement.Application.Models.Notification
{
    public record EmailSendRequest(string to, string subject, string body);
}
