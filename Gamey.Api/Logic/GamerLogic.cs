using Gamey.Api.DataLayer.Interfaces;
using Gamey.Api.Interfaces;
using Gamey.Api.Models;

namespace Gamey.Api.Logic
{
    public class GamerLogic(IGamerDataLayer _gamerDataLayer, IGameLogic _gameLogic, IActionHistoryLogic _actionHistory) : IGamerLogic
    {
        public async Task<Gamer> GetGamerById(Guid gamerId)
        {
            return await _gamerDataLayer.ReadByGamerId(gamerId);
        }

        public async Task<Gamer> GetGamerByEmail(string gamerEmail)
        {
            return await _gamerDataLayer.ReadByGamerEmail(gamerEmail);
        }

        public async Task<Gamer> AddGamer(NewGamer newGamer)
        {
            // if new add
            var game = await _gameLogic.GetGame();
            if (game == null) throw new Exception("GamerLogic.AddGamer() - game is null");

            var gamer = new Gamer
            {
                GameId = game.id.Value,
                GamerEmail = newGamer.GamerEmail,
                GamerName = newGamer.GamerName
            };

            gamer = await _gamerDataLayer.Save(gamer);

            // record login
            await _actionHistory.AddAction(game, gamer, ActionEnum.Login);

            return gamer;
        }
    }
}
