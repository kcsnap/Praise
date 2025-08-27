namespace Praise.WebApp.Logic
{
    public interface ISendPraiseReceivedEvent
    {
        void OnSendPraiseReceived(object sender, SendPraiseEventArgs e);
    }
}