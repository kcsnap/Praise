using Container = Microsoft.Azure.Cosmos.Container;

namespace Gamey.Api.DataLayer.Interfaces
{
    public interface IGameyDbConnection
    {
        Container GameContainer { get; set; }
        Container GameEventContainer { get; set; }
        Container AchievementContainer { get; set; }
        Container BadgeContainer { get; set; }
        Container BadgeGroupContainer { get; set; }
        Container GamerContainer { get; set; }
    }
}
