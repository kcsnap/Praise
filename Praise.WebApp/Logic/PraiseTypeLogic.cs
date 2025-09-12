using Praise.Models;
using Praise.WebApp.Logic.Interfaces;

namespace Praise.WebApp.Logic
{
    public class PraiseTypeLogic(IPraiseApi praiseApi) : IPraiseTypeLogic
    {
        public async Task<IList<PraiseTypeMetadata>> GetPraiseTypes(IPraiseUserLogic praiseUser)
        {
            return await praiseApi.GetAsync<IList<PraiseTypeMetadata>>(praiseUser, PraiseApi.PraiseTypeUrl);
        }
    }
}
