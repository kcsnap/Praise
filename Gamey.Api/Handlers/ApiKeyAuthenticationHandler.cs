using Gamey.Api.Interfaces;
using Gamey.Api.Logic;
using Gamey.Api.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Gamey.Api.Handlers
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string ApiKeyHeaderName = "X-API-KEY";
        public const string ClientIdHeaderName = "X-CLIENT-ID";
        private readonly IConfiguration _configuration;
        private readonly IGameLogic _gameLogic;

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IConfiguration configuration,
            IGameLogic gameLogic)
            : base(options, logger, encoder, clock)
        {
            _configuration = configuration;
            _gameLogic = gameLogic;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKeyHeaderValues))
            {
                return Task.FromResult(AuthenticateResult.Fail("API Key was not provided."));
            }
            if (!Request.Headers.TryGetValue(ClientIdHeaderName, out var clientIdHeaderValues))
            {
                return Task.FromResult(AuthenticateResult.Fail("ClientId was not provided."));
            }

            var providedApiKey = apiKeyHeaderValues.FirstOrDefault();
            var providedClientId = clientIdHeaderValues.FirstOrDefault();

            Game game = null;
            Task.Run(async() => game = await _gameLogic.SetGame(providedClientId, providedApiKey)).Wait();
                        
            if (game == null)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid API Key and/or Client ID."));
            }


            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "ApiKeyUser") };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }

}
