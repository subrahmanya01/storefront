using System.Text.Json.Serialization;

namespace Storefront.UserService.Models.Common
{
    public class EmailBaseInfo
    {
        public string? FromName { get; set; }
        public string? FromEmail { get; set; }
        public List<string> ToEmailAddress { get; set; } = new List<string>();
        public string? Subject { get; set; } = string.Empty;
        public string HtmlContent { get; set; } = string.Empty;

    }

    public class EmailPayload
    {
        public string? FromEmail { get; set; }
        public string? FromName { get; set; }
        public List<EmailUser>? Recipients { get; set; }

        public string? Subject { get; set; }

        [JsonPropertyName("Text-part")]
        public string? TextPart { get; set; }

        [JsonPropertyName("Html-part")]
        public string? HtmlPart { get; set; }
    }

    public class EmailUser
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
    }

}
