using FluentValidation.AspNetCore;
using Identity.Domain.Models;
using Identity.Infrastructure;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Identity.API.Extensions;
using Identity.API.Domain;
using Constants.IdentityServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;

namespace Identity.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .ConfigureDatabase(Configuration)
                .ConfigureIdentity()
                .ConfigureIdentityServer(Configuration);

            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

            services.AddControllers();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = "http://localhost:12355";//IdentityServerConstants.ServerURL;
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.Events = new JwtBearerEvents()
                    {
                        OnTokenValidated = TokenValidationEvents.GetOnTokenValidatedEventHandler(services)
                    };
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = false,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        RequireSignedTokens = false
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyConstants.Admin, policy =>
                {
                    policy.RequireClaim(IdentityServerConstants.UserClaims.UserScopes,
                        IdentityServerConstants.Scopes.Identity);
                });
            });

            services.AddCors(conf =>
            {
                conf.AddPolicy("cors", policyConf =>
                {
                    policyConf.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IdentityContext identityContext)
        {
            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            identityContext.Database.Migrate();
            app.SeedIdentityServer();

            app.UseCors("cors");

            app.UseRouting();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
