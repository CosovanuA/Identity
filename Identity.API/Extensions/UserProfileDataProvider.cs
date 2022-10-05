using Identity.API.Domain.Interfaces;
using Identity.Domain.Models;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Constants.IdentityServer;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Identity.API.Extensions
{
    public class UserProfileDataProvider : IProfileDataProvider
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;

        public UserProfileDataProvider(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task SetClaims(ProfileDataRequestContext context)
        {
            var user = await userManager.GetUserAsync(context.Subject);
            IList<string> userRoles = null;
            
            if(user != null)
            {
                userRoles = await userManager.GetRolesAsync(user);
            }

            if(userRoles != null)
            {
                foreach (var userRole in userRoles)
                {
                    var role = await roleManager.FindByNameAsync(userRole);

                    if (role != null)
                    {
                        var roleClaims = await roleManager.GetClaimsAsync(role);

                        foreach (var roleClaim in roleClaims)
                        {
                            context.IssuedClaims.Add(new Claim(
                                IdentityServerConstants.UserClaims.UserScopes,
                                roleClaim.Value));
                        }
                    }
                }
            }

            context.IssuedClaims.Add(new Claim(
                IdentityServerConstants.UserClaims.UserConcurrencyStamp,
                user.ConcurrencyStamp));
        }
    }
}
