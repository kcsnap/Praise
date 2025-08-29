using Praise.Models;
using Praise.WebApp.Logic.Interfaces;

namespace Praise.WebApp.Logic
{
    public class PraiseTypeLogic(IPraiseApi praiseApi) : IPraiseTypeLogic
    {
        public async Task<IList<PraiseTypeMetadata>> GetPraiseTypes()
        {
            return await praiseApi.GetAsync<IList<PraiseTypeMetadata>>(PraiseApi.PraiseTypeUrl);
        }
    }
}
