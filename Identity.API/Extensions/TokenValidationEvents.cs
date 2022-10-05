using Identity.Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Constants.IdentityServer;

namespace Identity.API.Extensions
{
    public static class TokenValidationEvents
    {
        public static Func<TokenValidatedContext, Task> GetOnTokenValidatedEventHandler(IServiceCollection services)
        {
            return async (TokenValidatedContext context) =>
            {
                var userManager = services.BuildServiceProvider().GetRequiredService<UserManager<User>>();
                var claims = context.Principal?.Claims;
                string concurrencystampClaimValue = null;

                if (context.Principal == null)
                {
                    throw new Exception("Null principal");
                }

                if(claims != null)
                {
                    foreach (var claim in context.Principal?.Claims)
                    {
                        if (claim.Type.Equals(IdentityServerConstants.UserClaims.UserConcurrencyStamp))
                        {
                            concurrencystampClaimValue = claim.Value;
                        }
                    }
                }

                var user = await userManager.GetUserAsync(context.Principal);
                
                if(user != null &&
                    !string.IsNullOrEmpty(concurrencystampClaimValue) && 
                    !user.ConcurrencyStamp.Equals(concurrencystampClaimValue))
                {
                    context.Fail("ConcurrencyStamp expired");
                }

                return;
            };
        }
    }
}
