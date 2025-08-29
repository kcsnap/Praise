using brevo_csharp.Api;
using brevoClientConfig = brevo_csharp.Client;
using brevo_csharp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Praise.Api.Controllers;
using Praise.Models;

namespace Praise.Api.Logic
{
    public class EmailManagerLogic
    {
        //private readonly ILogging logging;
        private readonly TransactionalEmailsApi apiInstance;

        //public EmailManagerLogic(ILogging logging)
        public EmailManagerLogic()
        {
            //this.logging = logging;

            // BREVO...
            // Configure API key authorization: api-key
            brevoClientConfig.Configuration.Default.ApiKey.Add("api-key", "***REMOVED-S21***");

            apiInstance = new TransactionalEmailsApi();
        }

        public async System.Threading.Tasks.Task SendEmail(string toEmail, string bodyString)
        {
            // generate email
            var sendSmtpEmail = new SendSmtpEmail(); // SendSmtpEmail | Values to send a transactional email
            sendSmtpEmail.Sender = new SendSmtpEmailSender("kcox", "kcox@snapwire.co.uk");
            sendSmtpEmail.To = new List<SendSmtpEmailTo>()
            {
                new SendSmtpEmailTo(toEmail)
            };
            sendSmtpEmail.Subject = "You're being celbrated";
            sendSmtpEmail.HtmlContent = "<html><body><h4>" + bodyString + "</h4></body></html>";

            try
            {
                //await logging.WriteMessage("EMAIL SENT: " + bodyString);

                // Send a transactional email
                CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
            }
            catch (Exception e)
            {
                //await logging.WriteRecoverableError("EMAIL FAILURE: " + e.Message);
            }
        }
    }
}
