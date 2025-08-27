using Praise.WebApp.Logic.Events.EventBindings;

namespace Praise.WebApp.Logic
{
    public class PraiseLogic : IPraiseLogic
    {
        public event EventHandler<SendPraiseEventArgs> SendPraisePublished;

        public void PublishSendPraiseEvent(string message)
        {
            OnMessagePublished(new SendPraiseEventArgs(message));
        }

        protected virtual void OnMessagePublished(SendPraiseEventArgs e)
        {
            SendPraisePublished?.Invoke(this, e);
        }
    }

    public class SendPraiseEventArgs : EventArgs
    {
        public string Message { get; }

        public SendPraiseEventArgs(string message)
        {
            Message = message;
        }
    }
}

