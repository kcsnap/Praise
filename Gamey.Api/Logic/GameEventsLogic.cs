using Gamey.Api.DataLayer.Interfaces;
using Gamey.Api.Interfaces;
using Gamey.Api.Models;

namespace Gamey.Api.Logic
{
    public class GameEventsLogic(IGameLogic _gameLogic, IGameEventDataLayer _gameEventDataLayer) : IGameEventsLogic
    {
        protected IList<GameEvent> GameEvents { get; private set; }

        public async Task<GameEvent> GetGameEventByCode(string eventCode)
        {
            var game = await _gameLogic.GetGame();
            if(game == null || !game.id.HasValue) throw new Exception("Error retrieving game");

            return await _gameEventDataLayer.ReadByEventCode(game.id.Value, eventCode);
        }

        public async Task<IEnumerable<GameEvent>> GetGameEvents(Guid gameId)
        {
            return await _gameEventDataLayer.Read(gameId);
        }
    }
}
