using Gamey.Api.Models;

namespace Gamey.Api.DataLayer.Interfaces
{
    public interface IAchievementDataLayer
    {
        Task<IEnumerable<Achievement>> Read(Guid gamerId);
        Task<Achievement> Save(Achievement achievement);
    }
}