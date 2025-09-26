using Praise.Api.DataLayer.Interfaces;
using Praise.Models;

namespace Praise.WebApp.Logic
{
    public class PraiseLogic(IPraiseDetailsDataLayer praiseDetailsDataLayer) : IPraiseLogic
    {
        public event EventHandler<SendPraiseEventArgs> SendPraisePublished;

        public async void PublishSendPraiseEvent(PraiseDetails praiseDetails)
        {
            // commit to db then publish event
            try
            {
                await praiseDetailsDataLayer.Save(praiseDetails);

                // only send event on successful save
                await OnMessagePublished(new SendPraiseEventArgs(praiseDetails));
            }
            catch (Exception ex)
            {
                // TODO - logging and rethrow with inner
                throw new Exception("Failed Saving Praise", ex);
            }
        }

        protected virtual async Task OnMessagePublished(SendPraiseEventArgs e)
        {
            SendPraisePublished?.Invoke(this, e);
        }

        public async Task<IEnumerable<PraiseDetails>> GetPraiseDetails(string email, bool isSender, int? pageNumber, int? numberPerPage)
        {
            if(isSender)
                return await praiseDetailsDataLayer.ReadByPraiseSenderEmail(email, pageNumber, numberPerPage);
            else
                return await praiseDetailsDataLayer.ReadByPraiseReceiverEmail(email, pageNumber, numberPerPage);
        }
    }

    public class SendPraiseEventArgs : EventArgs
    {
        public PraiseDetails PraiseDetails { get; private set; }
        
        public SendPraiseEventArgs(PraiseDetails _praiseDetails)
        {
            PraiseDetails = _praiseDetails;
        }
    }
}

