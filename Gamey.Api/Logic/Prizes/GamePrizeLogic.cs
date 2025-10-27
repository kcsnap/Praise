using Gamey.Api.DataLayer.Interfaces;
using Gamey.Api.Interfaces;
using Gamey.Api.Models;
using Gamey.Models;

namespace Gamey.Api.Logic.Prizes
{
    public class GamePrizeLogic(IPrizeGroupDataLayer _gamePrizeDataLayer, IGamerEventDataDataLayer _gamerEventDataDataLayer, IGameEventsLogic _gameEventLogic,  ITokenWalletLogic _tokenWalletLogic) : IPrizeGroupLogic
    {
        public async Task<IEnumerable<PrizeGroups>> GetPrizesAvailableForGame(Guid gameId)
        {
            var prizes = await _gamePrizeDataLayer.Read(gameId);
            return prizes;
        }
        public async Task<PrizeGroups> GetPrizeEarnedForEvent(Guid gameId, Guid gamerId, GameEvent gameEventCode)
        {
            // retrieve prize data for this gamer and event            
            var prize = await _gamePrizeDataLayer.Read(gameId, gameEventCode.id.Value);

            // if no prizes then return
            if (prize == null) return null;

            //*********************************************************
            // ONLY PRIZE ATM IS CRYPTO! IN FUTURE BELOW FUNCTIONALITY
            // SHOULD BE ADDED TO FACTORY TO CATER FOR OTHER TYPES OF 
            // PRIZES (i.e. cash, points, discounts, etc)! (Similar to badges)
            //*********************************************************

            // check if wallet exists, if not create
            var newWallet = await _tokenWalletLogic.GetOrCreateWallet(gamerId);

            // reteieve the current count for this event and gamer
            var gamerEventData = await _gamerEventDataDataLayer.Read(gamerId, gameEventCode.id.Value, prize.id.Value);

            if(gamerEventData == null)
            {
                // no record exists so create one
                gamerEventData = new GamerEventData
                {
                    GamerId = gamerId,
                    GameEventId = gameEventCode.id.Value,
                    PrizeGroupDetailsId = prize.id.Value,
                };
            }

            // check if last event date is not today (null is new so award), then award crypto
            // Crypto can only be earned once per day
            if (gamerEventData.LastEventDate.Date != DateTime.Now.Date)
            {
                // update prize amount
                await _tokenWalletLogic.SendToken(gamerId, prize.PrizeAmount);
                gamerEventData.LastEventDate = DateTime.Now;
                await _gamerEventDataDataLayer.Save(gamerEventData);

                return prize;
            }
            else
            {
                // update last action date
                gamerEventData.LastEventDate = DateTime.Now;
                await _gamerEventDataDataLayer.Save(gamerEventData);

                // return null as already earned for today...
                return null;
            }
        }
    }
}
