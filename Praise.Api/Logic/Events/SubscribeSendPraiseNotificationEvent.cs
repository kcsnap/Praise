using Praise.Api.Logic.Interfaces;
using Praise.WebApp.Logic;
using System.IO;
using System.Runtime.CompilerServices;

namespace Praise.Api.Logic.Events
{
    public class SubscribeSendPraiseNotificationEvent(IHttpContextAccessor contextAccessor, IEmailManagerLogic emailManagerLogic) : ISendPraiseReceivedEvent
    {
        public void OnSendPraiseReceived(object sender, SendPraiseEventArgs e)
        {
            StreamReader reader = new StreamReader(contextAccessor.HttpContext.Request.Body);
            string text = Task.Run(reader.ReadToEndAsync).Result;

            string subject = "Congratulations! You're being celebrated!";
            string body = e.PraiseType.GetDescription() + "<br/><br/>from " + e.FromName;

            // extract to and type from payload 
            Task.Run(async() => emailManagerLogic.SendEmail(e.ToEmail, subject, body));

            // TODO - send notifications
            Console.WriteLine($"Received message from {((PraiseLogic)sender).GetType().Name}: {e.ToEmail}");
        }
    }
}
