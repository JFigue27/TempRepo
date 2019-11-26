using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using Reusable;
using Microsoft.AspNet.Identity.EntityFramework;
using Reusable.Auth;
using BusinessSpecificLogic.Logic;
using Ninject;
using ReusableWebAPI.App_Start;
using System.Collections.Generic;

namespace ReusableWebAPI.Auth
{
    internal class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private UserLogic userLogic;

        public SimpleAuthorizationServerProvider()
        {
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return base.ValidateClientAuthentication(context);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            List<Claim> claims = new List<Claim>(context.Identity.Claims);

            context.AdditionalResponseParameters.Add("Roles", claims.Find(c => c.Type == "role").Value);
            context.AdditionalResponseParameters.Add("UserKey", claims.Find(c => c.Type == "userID").Value);
            context.AdditionalResponseParameters.Add("UserName", claims.Find(c => c.Type == "userName").Value);
            context.AdditionalResponseParameters.Add("DisplayName", claims.Find(c => c.Type == "displayName").Value);
            context.AdditionalResponseParameters.Add("Email", claims.Find(c => c.Type == "email").Value);

            return base.TokenEndpoint(context);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            using (AuthRepository _repo = new AuthRepository())
            {
                IdentityUser user = await _repo.FindUser(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "User name or password is incorrect.");
                    return;
                }
            }


            userLogic = NinjectWebCommon.CreateKernel().Get<UserLogic>();

            CommonResponse response = userLogic.GetByName(context.UserName);
            if (response.ErrorThrown)
            {
                context.SetError(response.ResponseDescription);
                return;
            }

            User theUser = (User)response.Result;

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("role", theUser.Role ?? ""));
            identity.AddClaim(new Claim("userID", theUser.id.ToString()));
            identity.AddClaim(new Claim("userName", theUser.UserName));
            identity.AddClaim(new Claim("email", theUser.Email ?? ""));
            identity.AddClaim(new Claim("displayName", theUser.Value ?? ""));

            context.Validated(identity);
        }
    }
}