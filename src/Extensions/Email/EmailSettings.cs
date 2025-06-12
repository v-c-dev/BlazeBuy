namespace BlazeBuy.Extensions.Email
{
    public sealed class EmailSettings
    {
        public string SendGridApiKey { get; init; } = default!;
        public string FromAddress { get; init; } = "noreply@yumblazor.com";
        public string FromName { get; init; } = "Yum Blazor";
    }
}
