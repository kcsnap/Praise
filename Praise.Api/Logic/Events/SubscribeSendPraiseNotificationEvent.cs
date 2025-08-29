using Praise.Api.Logic.Interfaces;
using Praise.WebApp.Logic;
using System.Xml.Linq;

namespace Praise.Api.Logic.Events
{
    public class SubscribeSendPraiseNotificationEvent : ISendPraiseReceivedEvent
    {
        public void OnSendPraiseReceived(object sender, SendPraiseEventArgs e)
        {
            // TODO - send notifications
            Console.WriteLine($"Received message from {((PraiseLogic)sender).GetType().Name}: {e.Message}");
        }
    }
}
