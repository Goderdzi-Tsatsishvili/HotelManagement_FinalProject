
using HotelManagement.Application.Models.Notification;

namespace HotelManagement.Application.Contracts.Service
{
    public interface IEmailService
    {
        Task<EmailSendResponse> Send(string to, string subject, string body);
    }
}
