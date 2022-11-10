using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Authentication.Cookies;

using WebControllers.Controllers;
using Service;
using Presenter;
using DataAccessInterface;
using DataAccessSQLServer;
using AuthService;
using Microsoft.AspNetCore.Http;
using System.Security.Authentication;

namespace WebController
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = "/api/v1/users/login";
                        options.Cookie.Name = "auth_cookie";
                        options.AccessDeniedPath = "/static/index.html";
                    });

            // Swashbuckle
            services.AddSwaggerGen();

            services.AddTransient<IConverterService, ConverterService>();
            services.AddTransient<IAuthService, AuthService.AuthService>();
            services.AddSingleton<ITextRenderer>(new WinFormsTextRendererAdapter());
            services.AddSingleton<IRepositoryFactory>(new DataAccessSQLServer.RepositoryFactory
            (
                new DbContextFactory(),
                "192.168.10.104",
                "thedotfactory_db",
                "SA",
                "P@ssword"
            ));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Swashbuckle
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api/v1/{documentname}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api/v1/v1/swagger.json", "My Cool API V1");
                c.RoutePrefix = "api/v1";
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
