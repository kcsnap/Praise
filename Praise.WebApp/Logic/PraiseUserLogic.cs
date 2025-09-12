using Microsoft.AspNetCore.Components.Authorization;
using Praise.Models;
using Praise.WebApp.Logic.Interfaces;

namespace Praise.WebApp.Logic
{

    public class PraiseUserLogic : IPraiseUserLogic
    {
        private readonly AuthenticationStateProvider _auth;
        public PraiseUserLogic(AuthenticationStateProvider auth)
        {
            _auth = auth;
            Task.Run(async() => await GetCurrentUserAsync());
        }

        public PraiseUser PraiseUser { get; private set; }

        public async Task GetCurrentUserAsync()
        {
            var user = (await _auth.GetAuthenticationStateAsync()).User;

            var nickname = user.Claims.First(c => c.Type == "nickname").Value;
            var email = user.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
            var userId = user.Claims.First(c => c.Type == "sid").Value;

            PraiseUser = new PraiseUser()
            {
                DisplayName = nickname,
                Email = email, 
                UserId = userId
            };
        }
    }
}
