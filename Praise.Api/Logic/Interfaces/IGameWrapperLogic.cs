using Gamey.Api.Models;

namespace Praise.Api.Logic.Interfaces
{
    public interface IGameWrapperLogic
    {
        Task RegisterGamer(IPraiseUserLogic _praiseUserLogic);
        Task<IEnumerable<Achievement>> GetAchievements(IPraiseUserLogic _praiseUserLogic);
        Task<IEnumerable<BadgeGroupDetails>> GetAllBadges(IPraiseUserLogic _praiseUserLogic);
    }
}