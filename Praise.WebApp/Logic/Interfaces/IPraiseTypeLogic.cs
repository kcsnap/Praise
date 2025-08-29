using Praise.Models;

namespace Praise.WebApp.Logic.Interfaces
{
    public interface IPraiseTypeLogic
    {
        Task<IList<PraiseTypeMetadata>> GetPraiseTypes();
    }
}
