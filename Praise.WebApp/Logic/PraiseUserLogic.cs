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
        }

        public PraiseUser PraiseUser 
        {
            get
            {
                var authState = Task.Run(async () => await _auth.GetAuthenticationStateAsync());
                var user = authState.Result.User;

                var nickname = user.Claims.First(c => c.Type == "nickname").Value;
                var email = user.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
                var userId = user.Claims.First(c => c.Type == "sid").Value;

                var _user = new PraiseUser()
                {
                    DisplayName = nickname,
                    Email = email,
                    UserId = userId
                };

                return _user;
            }
        }
    }
}
