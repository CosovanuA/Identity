using Identity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Identity.API.Infrastructure.Factories
{
    public class IdentityContextDesignFactory : IDesignTimeDbContextFactory<IdentityContext>
    {
        public IdentityContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .Build();

            var optionsBuilder = new DbContextOptionsBuilder<IdentityContext>();

            optionsBuilder.UseSqlServer(config["ConnectionStrings:DefaultConnectionString"], sqlServerOptionsAction: o => o.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name));

            return new IdentityContext(optionsBuilder.Options);
        }
    }
}
