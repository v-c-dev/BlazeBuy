using BlazeBuy.Extensions.Email;
using BlazeBuy.Models;
using BlazeBuy.Services.Interfaces;
using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace BlazeBuy.Services
{
    internal sealed class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _log;
        private readonly EmailSettings _settings;
        private readonly SendGridClient _client;
        private readonly EmailAddress _from;

        public EmailService(IOptions<EmailSettings> options,
                            ILogger<EmailService> log)
        {
            _log = log;
            _settings = options.Value;

            _client = new SendGridClient(_settings.SendGridApiKey);
            _from = new EmailAddress(_settings.FromAddress, _settings.FromName);
        }

        public async Task SendOrderConfirmationAsync(Order order, CancellationToken ct = default)
        {
            var to = new EmailAddress(order.Email, order.Name);

            var msg = MailHelper.CreateSingleEmail(
                          _from,
                          to,
                          $"Order #{order.Id} Confirmation",
                          $"Thanks, {order.Name}! Your total is {order.Total:C}.",
                          $"""
                      <h1>Thank you, {order.Name}!</h1>
                      <p>We have received your order <strong>#{order.Id}</strong>.</p>
                      <p><b>Total:</b> {order.Total:C}</p>
                      <p>Status: {order.Status}</p>
                      """);

            var resp = await _client.SendEmailAsync(msg, ct);
            _log.LogInformation("SendGrid status {Status} for order {OrderId}",
                                resp.StatusCode, order.Id);
        }

        public async Task SendPasswordResetAsync(string email, string link, CancellationToken ct = default)
        {
            var msg = MailHelper.CreateSingleEmail(
                          _from,
                          new EmailAddress(email),
                          "Password reset",
                          $"Use this link to reset your password: {link}",
                          $"<p>Click <a href=\"{link}\">here</a> to reset your password.</p>");

            await _client.SendEmailAsync(msg, ct);
        }

        public async Task SendGenericAsync(string to, string subject, string html, CancellationToken ct = default)
        {
            var plain = System.Text.RegularExpressions.Regex.Replace(html, "<.*?>", string.Empty);

            var msg = MailHelper.CreateSingleEmail(
                          _from, new EmailAddress(to), subject, plain, html);

            await _client.SendEmailAsync(msg, ct);
        }
    }
}
