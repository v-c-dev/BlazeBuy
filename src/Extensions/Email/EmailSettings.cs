namespace BlazeBuy.Extensions.Email
{
    public sealed class EmailSettings
    {
        public string SendGridApiKey { get; init; } = default!;
        public string FromAddress { get; init; }
        public string FromName { get; init; }
        public string? DataResidency { get; set; }
    }
}
