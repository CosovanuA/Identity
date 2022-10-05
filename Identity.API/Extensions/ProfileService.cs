using Identity.API.Domain.Interfaces;
using Identity.Domain.Models;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.API.Extensions
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;

        public ProfileService(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            IProfileDataProvider provider = new UserProfileDataProvider(userManager, roleManager);
            await provider.SetClaims(context);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await userManager.GetUserAsync(context.Subject);
            context.IsActive = user != null;
        }
    }
}
