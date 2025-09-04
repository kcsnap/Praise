
using Praise.Models;

namespace Praise.WebApp.Logic
{
    public interface IPraiseLogic
    {
        event EventHandler<SendPraiseEventArgs> SendPraisePublished;

        void PublishSendPraiseEvent(string toEmail, PraiseTypeEnum praiseType, string fromName, string fromEmail);
    }
}