using Gamey.Api.Models;

namespace Gamey.Api.DataLayer.Interfaces
{
    public interface IBadgeGroupDataLayer
    {
        Task<IEnumerable<BadgeGroupDetails>> Read(Guid gameId);
        Task<IEnumerable<BadgeGroupDetails>> ReadByEventCode(Guid gameId, Guid gameEventCodeId);
    }
}