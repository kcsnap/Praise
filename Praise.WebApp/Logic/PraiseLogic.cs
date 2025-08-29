using Praise.Models;
using Praise.WebApp.Logic.Interfaces;

namespace Praise.WebApp.Logic
{
    public class PraiseLogic(IPraiseApi praiseApi) : IPraiseLogic
    {
        public async Task<PraiseSent> SendPraise(string message)
        {
            return await praiseApi.PostAsync<PraiseSent>(PraiseApi.PraiseUrl);
        }
    }
}