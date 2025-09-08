using Praise.Api.Logic.Interfaces;
using Praise.WebApp.Logic;
using System.Xml.Linq;

namespace Praise.Api.Logic.Events
{
    public class SubscribeSendPraiseBadgeEvent : ISendPraiseReceivedEvent
    {
        public void OnSendPraiseReceived(object sender, SendPraiseEventArgs e)
        {
            // TODO - check badges
            Console.WriteLine($"Received message from {((PraiseLogic)sender).GetType().Name}: {e.PraiseDetails.ToEmail}");
        }
    }
}
