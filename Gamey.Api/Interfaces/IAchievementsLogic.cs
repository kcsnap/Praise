using Gamey.Api.Models;

namespace Gamey.Api.Interfaces
{
    public interface IAchievementsLogic
    {
        Task<bool> Exists(Guid gamerId, Guid badgeId);
        Task<IEnumerable<Achievement>> GetAchievementsForGamer(Guid gamerId);
        Task<Achievement> UpdateAchievement(Guid gamerId, Guid achievementId);
        Task<Achievement> AddAchievement(Guid gamerId, Guid badgeId);

    }
}