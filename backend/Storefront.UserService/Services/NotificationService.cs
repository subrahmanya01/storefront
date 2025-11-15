using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Storefront.UserService.Models.Common;
using Storefront.UserService.Models.Request;
using Storefront.UserService.Models.Types;
using System.Net.Http.Headers;
using System.Text;

namespace Storefront.UserService.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<StoreInfo> _storeInfo;
        
        public NotificationService(IConfiguration configuration, IOptions<StoreInfo> storeInfo, IHttpClientFactory httpClientFactory, ILogger<NotificationService> logger)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _storeInfo = storeInfo;
        }

        public async Task<bool> SendVerifyEmailMail(string email, string UniqueIdentifier)
        {
            var emailTemplate = await GetEmailTemplateString(EmailTemplateTypes.VerifyEmail);
            if (emailTemplate == null)
            {
                _logger.LogError("Email template not found : Contact Email");
                return false;
            }
            var modifiedHtml = emailTemplate
                .Replace("[Verification Link]", $"{_storeInfo.Value.Website}/verify-email/{UniqueIdentifier}")

                .Replace("[Current Year]", DateTime.Now.Year.ToString())
                .Replace("[Your Store Name]", _storeInfo.Value.Name)
                .Replace("[Your Store Address]", _storeInfo.Value.Address)
                .Replace("[Your Website]", _storeInfo.Value.Website);
            var requestObject = new EmailBaseInfo
            {
                FromName = _storeInfo.Value.Name,
                FromEmail = _storeInfo.Value.MainEmail,
                ToEmailAddress = new List<string> { email },
                Subject = "Password Reset",
                HtmlContent = modifiedHtml
            };
            return await SendEmail(requestObject);
        }


        public async Task<bool> SendResetPasswordEmail(string email, string UniqueIdentifier)
        {
            var emailTemplate = await GetEmailTemplateString(EmailTemplateTypes.PasswordReset);
            if (emailTemplate == null)
            {
                _logger.LogError("Email template not found : Contact Email");
                return false;
            }
            var modifiedHtml = emailTemplate
                .Replace("[Reset Password Link]", $"{_storeInfo.Value.Website}/reset-password/{UniqueIdentifier}")
                .Replace("[Link Expiry Time]", "30 min")

                .Replace("[Current Year]", DateTime.Now.Year.ToString())
                .Replace("[Your Store Name]", _storeInfo.Value.Name)
                .Replace("[Your Store Address]", _storeInfo.Value.Address)
                .Replace("[Your Website]", _storeInfo.Value.Website);
            var requestObject = new EmailBaseInfo
            {
                FromName = _storeInfo.Value.Name,
                FromEmail = _storeInfo.Value.MainEmail,
                ToEmailAddress = new List<string> { email },
                Subject = "Password Reset",
                HtmlContent = modifiedHtml
            };
            return await SendEmail(requestObject);
        }

        public async Task<bool> SendContactEmail(ContactRequest request)
        {
            var emailTemplate = await GetEmailTemplateString(EmailTemplateTypes.ContactEmail);
            if (emailTemplate == null) {
                _logger.LogError("Email template not found : Contact Email");
                return false;
            }
            var modifiedHtml = emailTemplate
                .Replace("[User's Name]", request.Name)
                .Replace("[User's Email]", request.Email)
                .Replace("[User's Phone]", request.PhoneNumber)
                .Replace("[User's Message]", request.Message)

                .Replace("[Current Year]", DateTime.Now.Year.ToString())
                .Replace("[Your Store Name]", _storeInfo.Value.Name)
                .Replace("[Your Store Address]", _storeInfo.Value.Address)
                .Replace("[Your Website]", _storeInfo.Value.Website);
            var requestObject = new EmailBaseInfo
            {
                FromName = request.Name,
                FromEmail = request.Email,
                ToEmailAddress = _storeInfo.Value.CompanySupportEmails!,
                Subject = "User Contating You",
                HtmlContent = modifiedHtml
            };
            return await SendEmail(requestObject);
        }

        private async Task<string?> GetEmailTemplateString(string emailTemplateType)
        {
            var  path =  Path.Combine(AppContext.BaseDirectory, "EmailTemplates", emailTemplateType);
            if (File.Exists(path))
            {
                return await File.ReadAllTextAsync(path);
            }
            return null;
        }

        private async Task<bool> SendEmail(EmailBaseInfo request)
        {
            var mailJetConfigurations = _configuration.GetSection("MailJet");
            var privateKey = mailJetConfigurations.GetValue<string>("PrivateKey");
            var publicKey = mailJetConfigurations.GetValue<string>("PublicKey");
            var apiUrl = mailJetConfigurations.GetValue<string>("ApiUrl");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{publicKey}:{privateKey}")));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var recipients = request.ToEmailAddress.Select(x => new EmailUser { Name = x, Email = x });

                var payload = new EmailPayload
                {
                    FromEmail = request.FromEmail,
                    FromName = request.FromName,
                    Recipients = recipients.ToList(),
                    Subject = request.Subject,
                    TextPart = "",
                    HtmlPart = request.HtmlContent
                };

                var requestBody = new
                {
                    Messages = new[]
                    {
                        new
                        {
                            From = new { Email = request.FromEmail, Name = request.FromName },
                            To = recipients.ToList(),
                            Subject = "Reset Your Password",
                            HTMLPart = request.HtmlContent
                        }
                    }
                 };


                var jsonPayload = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    string responseContent = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        return false;
                    }
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError("Exception occured while sending email {message}", ex.Message);
                }
            }
            return true;
        }
    }
}
