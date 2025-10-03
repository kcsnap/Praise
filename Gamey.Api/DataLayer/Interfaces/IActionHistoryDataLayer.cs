using Gamey.Api.Models;

namespace Gamey.Api.DataLayer.Interfaces
{
    public interface IActionHistoryDataLayer
    {
        Task<ActionHistory> Save(ActionHistory actionHistory);
    }
}