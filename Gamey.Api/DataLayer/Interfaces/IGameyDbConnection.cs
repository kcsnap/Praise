using Container = Microsoft.Azure.Cosmos.Container;

namespace Gamey.Api.DataLayer.Interfaces
{
    public interface IGameyDbConnection
    {
        Container GameContainer { get; set; }
        Container GameEventContainer { get; set; }

    }
}
