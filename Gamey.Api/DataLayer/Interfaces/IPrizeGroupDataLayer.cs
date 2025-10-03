using Gamey.Models;

namespace Gamey.Api.DataLayer.Interfaces
{
    public interface IPrizeGroupDataLayer
    {
        Task<IEnumerable<PrizeGroups>> Read(Guid gameId);
        Task<PrizeGroups> Read(Guid gameId, Guid gameEventCodeId);
    }
}