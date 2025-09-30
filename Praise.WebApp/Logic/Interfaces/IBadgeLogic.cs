using Praise.Models;

namespace Praise.WebApp.Logic.Interfaces
{
    public interface IBadgeLogic
    {
        Task<IEnumerable<BadgeGroupViewModel>> GetBadges(IPraiseUserLogic praiseUser);
    }
}