using Praise.Models;

namespace Praise.WebApp.Logic
{
    public class PraiseLogic : IPraiseLogic
    {
        public event EventHandler<SendPraiseEventArgs> SendPraisePublished;

        public async void PublishSendPraiseEvent(string toEmail, PraiseTypeEnum praiseType, string fromName, string fromEmail)
        {
            await OnMessagePublished(new SendPraiseEventArgs(toEmail, praiseType, fromName, fromEmail));
        }

        protected virtual async Task OnMessagePublished(SendPraiseEventArgs e)
        {
            SendPraisePublished?.Invoke(this, e);
        }
    }

    public class SendPraiseEventArgs : EventArgs
    {
        public string ToEmail { get; }
        public PraiseTypeEnum PraiseType { get; set; }
        public string FromName { get; set; }
        public string FromEmail { get; set; }

        public SendPraiseEventArgs(string toEmail, PraiseTypeEnum praiseType, string fromName, string fromEmail)
        {
            ToEmail = toEmail;
            FromName = fromName;
            FromEmail = fromEmail;
            PraiseType = praiseType;
        }
    }
}

