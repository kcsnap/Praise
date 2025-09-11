using Praise.Models;
using Praise.Models.Generic;
using Praise.WebApp.Logic.Interfaces;

namespace Praise.WebApp.Logic
{
    public class PraiseLogic(IPraiseApi praiseApi, ILogger<PraiseLogic> _logger) : IPraiseLogic
    {
        public async Task<IEnumerable<PraiseDetails>> GetPraisesReceived()
        {
            var sent = await praiseApi.GetAsync<IEnumerable<PraiseDetails>>(PraiseApi.PraiseUrl, new Paging() { NumberPerPage = 10 }, "issender=false");
            return sent;
        }

        public async Task<IEnumerable<PraiseDetails>> GetPraisesSent()
        {
            var received = await praiseApi.GetAsync<IEnumerable<PraiseDetails>>(PraiseApi.PraiseUrl, new Paging() { NumberPerPage = 10 }, "issender=true");
            return received;
        }

        public async Task<PraiseDetails> SendPraise(string fromEmail, string toEmail, PraiseTypeMetadata praiseTypeMetadata)
        {
            try
            {
                return await praiseApi.PostAsync(PraiseApi.PraiseUrl, new PraiseDetails
                {
                    Text = praiseTypeMetadata.Description,
                    ImageUrl = praiseTypeMetadata.ImageUrl,
                    PraiseDate = DateTime.UtcNow,
                    ToEmail = toEmail,
                    FromEmail = fromEmail,
                    PraiseType = praiseTypeMetadata.Type
                });
            }
            catch (Exception ex)
            {
                var msg = "Error praising " + toEmail + " from " + fromEmail;
                _logger.LogError(msg, ex);
                throw new Exception(msg, ex);
            }
        }
    }
}