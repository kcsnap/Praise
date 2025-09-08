using Praise.Api.Logic.Interfaces;
using Praise.WebApp.Logic;

namespace Praise.Api.Logic.Events
{
    public class SubscribeSendPraiseNotificationEvent(IHttpContextAccessor contextAccessor, IEmailManagerLogic emailManagerLogic) : ISendPraiseReceivedEvent
    {
        public void OnSendPraiseReceived(object sender, SendPraiseEventArgs e)
        {
            //StreamReader reader = new StreamReader(contextAccessor.HttpContext.Request.Body);
            //string text = Task.Run(reader.ReadToEndAsync).Result;

            string subject = "Congratulations! You're being celebrated!";
            string body = e.PraiseDetails.PraiseType.GetDescription() + "<br/><br/>from " + e.PraiseDetails.FromEmail;

            // extract to and type from payload 
            Task.Run(async() => emailManagerLogic.SendEmail(e.PraiseDetails.ToEmail, subject, body));

            // TODO - send notifications
            Console.WriteLine($"Received message from {((PraiseLogic)sender).GetType().Name}: {e.PraiseDetails.ToEmail}");
        }
    }
}
