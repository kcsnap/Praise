using Praise.WebApp.Logic;

namespace Praise.Api.Logic.Interfaces
{
    public interface ISendPraiseReceivedEvent
    {
        void OnSendPraiseReceived(object sender, SendPraiseEventArgs e);
    }
}