using Praise.Models;

namespace Praise.WebApp.Logic
{
    public interface IPraiseTypeLogic
    {
        Task<IList<PraiseTypeMetadata>> GetPraiseTypes();
    }
}
