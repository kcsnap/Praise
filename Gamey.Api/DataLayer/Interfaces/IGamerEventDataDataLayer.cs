using Gamey.Api.Models;

namespace Gamey.Api.DataLayer.Interfaces
{
    public interface IGamerEventDataDataLayer
    {   
        Task<GamerEventData> Read(Guid gamerId, Guid gameEventId, Guid groupId);
        Task<GamerEventData> Save(GamerEventData gamerEventData);
    }
}