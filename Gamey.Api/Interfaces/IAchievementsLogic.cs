using Gamey.Api.Models;

namespace Gamey.Api.Interfaces
{
    public interface IAchievementsLogic
    {
        Task<bool> Exists(Gamer gamer, string badgeId);
        Task<IEnumerable<Achievement>> GetAchievementsForGamer(string gamerId);
        Task<Achievement> UpdateAchievement(string gamerId, string achievementId);
        Task<Achievement> SaveAchievement(Achievement achievement);

    }
}