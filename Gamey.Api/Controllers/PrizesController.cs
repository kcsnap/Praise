using Gamey.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gamey.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class PrizesController(ITokenWalletLogic _tokenWalletLogic) : Controller
    {
        /// <summary>
        /// Return a list of Game authenticated to use.
        /// </summary>
        /// <returns></returns>        
        [HttpGet("gamers/{gamerId}/prizes")]
        public async Task<decimal> Get(Guid gamerId)
        {
            // ********************************************************************
            // THIS IS SPECIFIC TO CRYPTO - ONCE MORE PRIXZES BECOME AVAILABLE THIS
            // SHOULD EITHER BE SEPARATED AS A SPECIFICAL CRYPTO LOGIC OR FACTORY-ISED
            // ALONG WITH OTHER PRIZE TYPES
            // ********************************************************************

            var balance = await _tokenWalletLogic.GetBalance(gamerId);

            if (balance == null) throw new Exception("Error retrieving balance");

            return balance;
        }
    }
}
