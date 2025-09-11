using Microsoft.AspNetCore.Components.Authorization;
using Praise.WebApp.Logic.Interfaces;

namespace Praise.WebApp.Logic
{

    public class PraiseUser : IPraiseUser
    {
        private readonly AuthenticationStateProvider _auth;
        public PraiseUser(AuthenticationStateProvider auth) => _auth = auth;

        public async Task<string?> GetCurrentUserAsync()
        {
            var user = (await _auth.GetAuthenticationStateAsync()).User;
            return user.Identity?.IsAuthenticated == true ? user.Identity.Name : null;
        }
    }
}
