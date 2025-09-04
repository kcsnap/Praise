using Praise.Models;
using Praise.WebApp.Logic.Interfaces;

namespace Praise.WebApp.Logic
{
    public class PraiseLogic(IPraiseApi praiseApi) : IPraiseLogic
    {
        public async Task<PraiseSent> SendPraise(string fromEmail, string toEmail, PraiseTypeMetadata praiseTypeMetadata)
        {
            return await praiseApi.PostAsync(PraiseApi.PraiseUrl, new PraiseSent
            {
                Text = praiseTypeMetadata.Description,
                ImageUrl = praiseTypeMetadata.ImageUrl,
                PraiseDate = DateTime.UtcNow,
                To = toEmail,                
            });
        }
    }
}