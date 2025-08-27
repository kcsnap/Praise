using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Praise.WebApp.Logic
{
    public interface IPraiseAuthentication
    {
        Task Authenticate();
    }

    public class PraiseAuthentication : PageModel, IPraiseAuthentication
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public PraiseAuthentication(IHttpContextAccessor context)
        {
            _contextAccessor = context;
        }

        public async Task Authenticate()
        {
            var redirectUri = "/";
            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                .WithRedirectUri(redirectUri)
                .Build();

            var ctx = _contextAccessor.HttpContext;
            await ctx.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties); ;
        }
    }
}
