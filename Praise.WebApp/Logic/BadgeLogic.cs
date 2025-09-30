using Praise.Models;
using Praise.WebApp.Logic.Interfaces;

namespace Praise.WebApp.Logic
{
    public class BadgeLogic(IHttpContextAccessor _httpAccessor, ILogger<BadgeLogic> _logger, IPraiseApiWrapper praiseApi) : IBadgeLogic
    {
        /// <summary>
        /// Get badges for game and claimed date
        /// </summary>
        /// <param name="praiseUser"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BadgeGroupViewModel>> GetBadges(IPraiseUserLogic praiseUser)
        {
            var badges = await praiseApi.GetAsync<IEnumerable<BadgeGroupViewModel>>(praiseUser, PraiseApiWrapper.BadgeUrl);
            return badges;
        }

        public async Task ClaimBadge(IPraiseUserLogic praiseUser, Guid achievementId)
        {
            var path = string.Format(PraiseApiWrapper.AchievementsPatchUrl, achievementId);
            await praiseApi.PatchAsync(praiseUser, path);            
        }
    }
}