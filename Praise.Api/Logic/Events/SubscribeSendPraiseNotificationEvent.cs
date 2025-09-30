using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Praise.Api.Logic.Interfaces;
using Praise.WebApp.Logic;

namespace Praise.Api.Logic.Events
{
    public class SubscribeSendPraiseNotificationEvent(IHttpContextAccessor contextAccessor, IEmailManagerLogic emailManagerLogic, IConfiguration configuration) : ISendPraiseReceivedEvent
    {
        public void OnSendPraiseReceived(object sender, SendPraiseEventArgs e)
        {
            //StreamReader reader = new StreamReader(contextAccessor.HttpContext.Request.Body);
            //string text = Task.Run(reader.ReadToEndAsync).Result;
            var rootUrl = configuration["Email:CDNRoot"];

            string subject = "Congratulations! You're being celebrated!";
            string body = "<h1>" + e.PraiseDetails.PraiseType.GetDescription() + "</h1><br/>" +
                           "<img src=\"" + rootUrl + e.PraiseDetails.ImageUrl + "\"/><br/><br/>" +
                          "from " + e.PraiseDetails.FromEmail;

            // extract to and type from payload 
            Task.Run(async() => await emailManagerLogic.SendEmail(e.PraiseDetails.ToEmail, subject, body));
        }
    }
}
