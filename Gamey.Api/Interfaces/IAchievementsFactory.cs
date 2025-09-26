using Gamey.Api.Logic;
using Gamey.Api.Models;

namespace Gamey.Api.Interfaces
{
    public interface IAchievementsFactory
    {
        IAwardsLogic Get(BadgeGroupDetails badgeGroupDetails);
    }
}