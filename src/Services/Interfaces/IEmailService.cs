using BlazeBuy.Models;

namespace BlazeBuy.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendOrderConfirmationAsync(Order order, CancellationToken ct = default);
        Task SendPasswordResetAsync(string email, string resetLink, CancellationToken ct = default);
        Task SendGenericAsync(string to, string subject, string htmlBody, CancellationToken ct = default);
    }
}
