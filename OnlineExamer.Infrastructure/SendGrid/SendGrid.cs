using Microsoft.Extensions.Configuration;
using OnlineExamer.Models.Dtos.SendGrid;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace OnlineExamer.Infrastructure.SendGrid
{
    public class SendGrid
    {
        private readonly IConfiguration configuration;
        private readonly string ApiKey;

        public SendGrid(IConfiguration configuration)
        {
            this.configuration = configuration;
            ApiKey = configuration.GetSection("SendGrid").GetValue<string>("ApiKey");
        }

        public async Task<Response> SendEmailAsync(EmailModel emailModel)
        {
            SendGridClient client = new SendGridClient(ApiKey);
            EmailAddress from = new EmailAddress(emailModel.From, emailModel.Name);
            EmailAddress to = new EmailAddress(emailModel.To, "Nikolay");
            string plainTextContent = emailModel.Body;
            string htmlContent = emailModel.Body;
            SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, emailModel.Subject, plainTextContent, htmlContent);
            return await client.SendEmailAsync(msg);
        }
    }
}
