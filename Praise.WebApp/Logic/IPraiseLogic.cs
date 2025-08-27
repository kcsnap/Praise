
namespace Praise.WebApp.Logic
{
    public interface IPraiseLogic
    {
        event EventHandler<SendPraiseEventArgs> SendPraisePublished;

        void PublishSendPraiseEvent(string message);
    }
}