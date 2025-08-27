using Clerk.BackendAPI;
using Clerk.BackendAPI.Helpers.Jwks;
using Clerk.BackendAPI.Models.Components;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace ClerkAuth
{
    [TestClass]
    public sealed class Test1
    {
        

        [TestMethod]
        public async Task TestMethod1()
        {
            var options = new AuthenticateRequestOptions(
                secretKey: "***REMOVED-S21***",
                authorizedParties: new string[] { "https://example.com" }
            );

            var request = new DefaultHttpContext().Request;
            var requestState = await AuthenticateRequest.AuthenticateRequestAsync(request, options);

            Assert.IsTrue(requestState.IsAuthenticated);
        }
    }
}
