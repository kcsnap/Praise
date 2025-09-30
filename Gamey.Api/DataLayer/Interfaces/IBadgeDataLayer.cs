using Gamey.Api.Models;

namespace Gamey.Api.DataLayer.Interfaces
{
    public interface IBadgeDataLayer
    {
        Task<IEnumerable<BaseBadgeDetails>> Read(Guid badgeGroupId, BadgeType badgeType);
    }
}