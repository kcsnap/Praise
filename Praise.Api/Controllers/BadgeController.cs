using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Praise.Api.Logic.Interfaces;
using Praise.Models;

namespace Praise.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class BadgeController(ILogger<GameController> _logger, IGameWrapperLogic _gameLogic, IPraiseUserLogic _praiseUserLogic) : Controller
    {
        [HttpGet(Name = "get badges")]
        public async Task<IEnumerable<BadgeGroupViewModel>> Get()
        {
            try
            {
                // ***********************************************************************************
                // todo - these need to be checked agaisnt list of ALL badges (Get working but
                // soon after optimise with caching as requesting these all the time is pretty crap)
                var allBadges = await _gameLogic.GetAllBadges(_praiseUserLogic);
                var flatenedBadges = allBadges.SelectMany(bg => bg.BadgeDetails);
                // ***********************************************************************************

                // get current achievements
                var achievements = await _gameLogic.GetAchievements(_praiseUserLogic);

                // get badge details of those unclaimed
                var badgeGroupDetails = new List<BadgeGroupViewModel>();
                foreach(var badgeGroup in allBadges)
                {    
                    var tempBadges = new List<BadgeDetailsViewModel>();
                    foreach (var badge in badgeGroup.BadgeDetails)
                    {
                        var achievement = achievements.SingleOrDefault(ua => ua.BadgeId == badge.id);
                        
                        tempBadges.Add(new BadgeDetailsViewModel
                        {
                            BadgeId = badge.id.Value,
                            AchievementId = achievement != null ? achievement.id.Value : null,
                            BadgeSubtitle = badge.BadgeSubtitle,
                            BadgeDateEarned = achievement != null ? achievement.DateAchieved : null,
                            BadgeDateClaimed = achievement != null ? achievement.DateClaimed : null,
                            BadgeImageUrl = badge.BadgeImageUrl,
                            BadgeType = badge.BadgeType
                        });
                    }

                    badgeGroupDetails.Add(new BadgeGroupViewModel
                    {
                        BadgegroupId = badgeGroup.id.Value,
                        BadgeTitle = badgeGroup.BadgeTitle,
                        BadgeDetails = tempBadges
                    });
                }

                return badgeGroupDetails;
            }
            catch (Exception ex)
            {
                var msg = "PostGame() Error: [See inner exception]";
                _logger.LogError(msg, ex);
                throw new Exception(msg, ex);
            }
        }
    }
}
