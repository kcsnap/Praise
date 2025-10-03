using Gamey.Api.DataLayer.Interfaces;
using Gamey.Api.Interfaces;
using Gamey.Api.Models;

namespace Gamey.Api.Logic
{
    public class ActionHistoryLogic(IActionHistoryDataLayer _actionHistoryDataLayer) : IActionHistoryLogic
    {
        public async Task AddAction(Game game, Gamer gamer, ActionEnum action)
        {
            await _actionHistoryDataLayer.Save(new ActionHistory
            {
                Action = action,
                GameId = game.id.Value,
                GamerId = gamer.id.Value,
                ActionDate = DateTime.UtcNow
            });
        }
    }
}
