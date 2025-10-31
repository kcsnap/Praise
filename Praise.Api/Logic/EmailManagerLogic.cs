using brevo_csharp.Api;
using brevo_csharp.Model;
using Praise.Api.Logic.Events;
using brevoClientConfig = brevo_csharp.Client;

namespace Praise.Api.Logic
{
    public class EmailManagerLogic : IEmailManagerLogic
    {
        //private readonly ILogging logging;
        private readonly TransactionalEmailsApi apiInstance;
        private readonly string apiKey;
        private readonly string emailFrom;

        //public EmailManagerLogic(ILogging logging)
        public EmailManagerLogic(IConfiguration configuration)
        {
            //this.logging = logging;

            apiKey = configuration["Email:ApiKey"];
            emailFrom = configuration["Email:EmailFrom"];

            // BREVO...
            // Configure API key authorization: api-key
            brevoClientConfig.Configuration.Default.ApiKey.Add("api-key", apiKey);

            apiInstance = new TransactionalEmailsApi();
        }

        public async System.Threading.Tasks.Task SendEmail(string toEmail, string subject, string bodyString)
        {
            // generate email
            var sendSmtpEmail = new SendSmtpEmail(); // SendSmtpEmail | Values to send a transactional email
            
            sendSmtpEmail.Sender = new SendSmtpEmailSender(emailFrom, emailFrom);
            sendSmtpEmail.To = new List<SendSmtpEmailTo>()
            {
                new SendSmtpEmailTo(toEmail)
            };
            sendSmtpEmail.Subject = subject;
            sendSmtpEmail.HtmlContent = "<html><body>" + bodyString + "</body></html>";

            try
            {
                // Send a transactional email
                CreateSmtpEmail result = await apiInstance.SendTransacEmailAsync(sendSmtpEmail);
            }
            catch (Exception e)
            {                
                throw new Exception(e.Message, e);
            }
        }
    }
}
