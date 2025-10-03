using Praise.Api.Logic.Interfaces;
using Praise.Models;

namespace Praise.Api.Logic
{

    public class PraiseUserLogic : IPraiseUserLogic
    {
        //private readonly AuthenticationStateProvider _auth;
        private readonly IHttpContextAccessor _contextAccessor;
        //public PraiseUserLogic(AuthenticationStateProvider auth)
        public PraiseUserLogic(IHttpContextAccessor contextAccessor)
        {
            //_auth = auth;
            _contextAccessor = contextAccessor;
        }

        PraiseUser _praiseUser;

        public PraiseUser PraiseUser 
        {
            get
            {
                //(await _auth.GetAuthenticationStateAsync()).User;

                //var nickname = user.Claims.First(c => c.Type == "nickname").Value;
                //var email = user.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
                //var userId = user.Claims.First(c => c.Type == "sid").Value;

                // get email from header context
                var email = _contextAccessor.HttpContext.Request.Headers["username"];

                _praiseUser = new PraiseUser()
                {
                    //DisplayName = nickname,
                    Email = email,
                    //UserId = userId
                };

                return _praiseUser;
            }
        }
    }
}
