using Identity.Domain.Models;
using Identity.Domain.Shared;
using Identity.Infrastructure.Factories.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure
{
    public class IdentityContext : IdentityDbContext<User, Role, int, 
        IdentityUserClaim<int>, IdentityUserRole<int> ,IdentityUserLogin<int>, 
        IdentityRoleClaim<int>, IdentityUserToken<int>>, 
        IUnitOfWork
    {
        public IdentityContext() : base() { }
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
