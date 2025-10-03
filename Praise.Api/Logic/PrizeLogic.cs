using Praise.Api.Logic.Interfaces;

namespace Praise.Api.Logic
{
    public class PrizeLogic(IGameWrapperLogic _gameWrapperLogic) : IPrizeLogic
    {
        public async Task<int> GetCryptoBalance(IPraiseUserLogic praiseUserLogic)
        {
            return await _gameWrapperLogic.GetCryptoBalance(praiseUserLogic);
        }
    }
}
