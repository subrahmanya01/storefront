using System.Text.Json.Serialization;

namespace Storefront.UserService.Models.Response
{
    public class AuthResponse
    {
        public AuthResponse(bool success, string message, string? token = null, DateTime? expirationTime = null, string? refreshToken = null)
        {
            Success = success;
            Message = message;
            Token = token;
            TokenExpiresAt = expirationTime;
            RefreshToken = refreshToken;
        }
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Token { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? TokenExpiresAt { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? RefreshToken { get; set; }
    }
}
