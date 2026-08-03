
using MimeKit;

namespace HotelManagement.Application.Contracts.Service
{
    public interface ISmtpService : IDisposable
    {
        Task ConnectAsync(string host, int port, bool useSsl);
        Task AuthenticateAsync(string username, string password);
        Task SendAsync(MimeMessage message);
        Task DisconnectAsync(bool quit);
    }
}
