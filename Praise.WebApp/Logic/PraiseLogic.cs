using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;
using Praise.Models;
using Praise.Models.Generic;
using Praise.WebApp.Logic.Interfaces;

namespace Praise.WebApp.Logic
{
    public class PraiseLogic(IHttpContextAccessor _httpAccessor, ILogger<PraiseLogic> _logger, IPraiseApi praiseApi) : IPraiseLogic
    {
        public async Task<IEnumerable<PraiseDetails>> GetPraisesReceived(IPraiseUser praiseUser)
        {
            var sent = await praiseApi.GetAsync<IEnumerable<PraiseDetails>>(praiseUser, PraiseApi.PraiseUrl, new Paging() { NumberPerPage = 10 }, "issender=false");
            return sent;
        }

        public async Task<IEnumerable<PraiseDetails>> GetPraisesSent(IPraiseUser praiseUser)
        {
            var received = await praiseApi.GetAsync<IEnumerable<PraiseDetails>>(praiseUser, PraiseApi.PraiseUrl, new Paging() { NumberPerPage = 10 }, "issender=true");
            return received;
        }

        public async Task<PraiseDetails> SendPraise(IPraiseUser praiseUser, string toEmail, PraiseTypeMetadata praiseTypeMetadata)
        {
            string fromEmail = string.Empty;

            try
            {
                fromEmail = await praiseUser.GetCurrentUserAsync();

                if (string.IsNullOrEmpty(fromEmail))
                    throw new Exception("PraiseLogic.SendPraise Current user cannot be null");

                _logger.LogInformation("Post praise from " + fromEmail + " to " + toEmail);
                return await praiseApi.PostAsync(praiseUser, PraiseApi.PraiseUrl, new PraiseDetails
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