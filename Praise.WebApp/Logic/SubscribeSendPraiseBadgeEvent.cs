using System.Xml.Linq;

namespace Praise.WebApp.Logic
{
    public class SubscribeSendPraiseBadgeEvent : ISendPraiseReceivedEvent
    {
        public void OnSendPraiseReceived(object sender, SendPraiseEventArgs e)
        {
            // TODO - check badges
            Console.WriteLine($"Received message from {((PraiseLogic)sender).GetType().Name}: {e.Message}");
        }
    }
}
