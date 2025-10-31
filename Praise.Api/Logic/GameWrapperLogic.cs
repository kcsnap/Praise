using Gamey.Api.Models;
using Gamey.Api.Models.Responses;
using Microsoft.Extensions.Options;
using Praise.Api.Configuration;
using Praise.Api.Logic.Interfaces;

namespace Praise.Api.Logic
{
    // wrapper class to gaming engine
    public class GameWrapperLogic : IGameWrapperLogic
    {
        // requires auth and specific user
        public static string _badgesUrl = "badges";
        public static string _gameUrl = "game";
        public static string _gameEventUrl = "gameevent";
        public static string _gamersUrl = "gamers"; 
        public static string _gamerUrl = "gamers/{0}"; // 0 = gamerEmail
        public static string _achievementsUrl = "gamers/{0}/achievements"; // 0 = gamerId
        public static string _achievementUrl = "gamers/{0}/achievements/{1}"; // 0 = gamerId, 1 = achievementId
        public static string _prizesUrl = "gamers/{0}/prizes"; // 0 = gamerId

        protected string bearerToken;
        protected readonly ILogger<GameWrapperLogic> _logger;
        protected readonly IHttpMethodWrapper _httpMethodWrapper;
        protected readonly GameApiOptions _gameApiOptions;

        protected Game _game;
        protected Gamer _gamer;

        public GameWrapperLogic(IOptions<GameApiOptions> gameApiOptions, ILogger<GameWrapperLogic> logger, IHttpMethodWrapper httpMethodWrapper)
        {
            // todo - refine Iconfiguration options to required section
            _logger = logger;
            _httpMethodWrapper = httpMethodWrapper;
            _gameApiOptions = gameApiOptions.Value;
        }

        protected async Task Initialise(IPraiseUserLogic _praiseUserLogic)
        {
            // ** can't inject IPraiseUserLogic in constructor due to Scoped vs Singleton DI **

            try
            {
                await _httpMethodWrapper.Initialise(_praiseUserLogic, _gameApiOptions.GameUri, _gameApiOptions.ClientId, _gameApiOptions.ApiKey);
                await LoadGame(_praiseUserLogic);
                await LoadGamer(_praiseUserLogic);
            }
            catch (Exception ex)
            {
                var msg = "GameApi.CallApi() => Error initialising client";
                _logger.LogError(msg, ex);
                throw new Exception(msg, ex);
            }
        }
        protected async Task LoadGame(IPraiseUserLogic _praiseUserLogic)
        {
            var gameResponse = await _httpMethodWrapper.GetAsync<Game>(_praiseUserLogic, _gameUrl);
            _game = gameResponse;
        }

        protected async Task LoadGamer(IPraiseUserLogic _praiseUserLogic)
        {
            var gamerPath = string.Format(_gamerUrl, _praiseUserLogic.PraiseUser.Email);
            var gamerResponse = await _httpMethodWrapper.GetAsync<Gamer>(_praiseUserLogic, gamerPath);
            _gamer = gamerResponse;
        }

        /// <summary>
        /// this is currenlty called everytime user logs in and reg just returns error if already registered
        /// </summary>
        /// <param name="_praiseUserLogic"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task RegisterGamer(IPraiseUserLogic _praiseUserLogic)
        {
            await Initialise(_praiseUserLogic);

            // if gamer doesn't exist then register
            if (_gamer == null)
            {
                // create the gamer
                var newGamer = new NewGamer
                {
                    GamerName = string.IsNullOrEmpty(_praiseUserLogic.PraiseUser.DisplayName) ? _praiseUserLogic.PraiseUser.Email : _praiseUserLogic.PraiseUser.DisplayName,
                    GamerEmail = _praiseUserLogic.PraiseUser.Email
                };

                // register gamer and update local ref
                _gamer = await _httpMethodWrapper.PostAsync<NewGamer, Gamer>(_praiseUserLogic, _gamersUrl, newGamer);
            }

            // api call to GameEvent
            var response = await _httpMethodWrapper.PostAsync<NewGameEvent, ResponseGameBadges>(_praiseUserLogic, _gameEventUrl, new NewGameEvent
            {
                EventCode = "register-event",
                GamerEmail = _praiseUserLogic.PraiseUser.Email
            });

            // api call to GameEvent
            var responseLogin = await _httpMethodWrapper.PostAsync<NewGameEvent, ResponseGameBadges>(_praiseUserLogic, _gameEventUrl, new NewGameEvent
            {
                EventCode = "login-event",
                GamerEmail = _praiseUserLogic.PraiseUser.Email
            });
        }

        public async Task SendPraiseEvent(IPraiseUserLogic _praiseUserLogic)
        {
            await Initialise(_praiseUserLogic);

            // api call to GameEvent
            var responseSendPraise = await _httpMethodWrapper.PostAsync<NewGameEvent, ResponseGameBadges>(_praiseUserLogic, _gameEventUrl, new NewGameEvent
            {
                EventCode = "send-praise-event",
                GamerEmail = _praiseUserLogic.PraiseUser.Email
            });
        }

        public async Task<IEnumerable<Achievement>> GetAchievements(IPraiseUserLogic _praiseUserLogic)
        {
            await Initialise(_praiseUserLogic);
            var path = string.Format(_achievementsUrl, _gamer.id);

            // api call to GameEvent
            var response = await _httpMethodWrapper.GetAsync<IEnumerable<Achievement>>(_praiseUserLogic, path);
            return response;
        }
        public async Task ClaimAchievement(IPraiseUserLogic _praiseUserLogic, Guid achievementId)
        {
            await Initialise(_praiseUserLogic);
            var path = string.Format(_achievementUrl, _gamer.id, achievementId);

            // api call to GameEvent
            await _httpMethodWrapper.PatchAsync(_praiseUserLogic, path);
        }

        public async Task<IEnumerable<BadgeGroupDetails>> GetAllBadges(IPraiseUserLogic _praiseUserLogic)
        {
            await Initialise(_praiseUserLogic);

            var response = await _httpMethodWrapper.GetAsync<IEnumerable<BadgeGroupDetails>>(_praiseUserLogic, _badgesUrl);
            return response;
        }

        public async Task<int> GetCryptoBalance(IPraiseUserLogic _praiseUserLogic)
        {
            await Initialise(_praiseUserLogic);

            var path = string.Format(_prizesUrl, _gamer.id);
            var response = await _httpMethodWrapper.GetAsync<int>(_praiseUserLogic, path);

            return response;
        }
    }
}
