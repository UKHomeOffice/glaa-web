using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GLAA.Domain;
using GLAA.Domain.Core;
using GLAA.Domain.Models;
using GLAA.Repository;
using GLAA.Services;
using GLAA.Services.Admin;
using GLAA.Services.Automapper;
using GLAA.Services.LicenceApplication;
using GLAA.Web.Core.Services;
using GLAA.Web.FormLogic;
using GLAA.Web.Helpers;
using GLAA.Web.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GLAA.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var user = Environment.GetEnvironmentVariable("DB_USER");
            var password = Environment.GetEnvironmentVariable("DB_PASS");

            //TODO: take DataSource intro environment
            var connectionString = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == EnvironmentName.Development ?
                Configuration["GLAAContext"] :
                $"Data Source=glaa-db-service.glaa-dev.svc.cluster.local,1433;Initial Catalog=GLAA_Core;Integrated Security=False;User Id={user};Password={password};MultipleActiveResultSets=True";

            services.AddDbContext<GLAAContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<SignInManager<GLAAUser>, SignInManager<GLAAUser>>();

            services.AddIdentity<GLAAUser, IdentityRole>()
                .AddEntityFrameworkStores<GLAAContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IFormDefinition, LicenceApplicationFormDefinition>();
            services.AddTransient<IFieldConfiguration, FieldConfiguration>();
            services.AddTransient<ISessionHelper, SessionHelper>();

            // automapper
            services.AddTransient<IMapper>(x => new AutoMapperConfig().Configure().CreateMapper());

            // http session
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // licence profile
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IEntityFrameworkRepository, EntityFrameworkRepositoryBase>();
            services.AddTransient<ILicenceRepository, LicenceRepository>();
            services.AddTransient<IStatusRepository, StatusRepository>();
            services.AddTransient<ILicenceApplicationPostDataHandler, LicenceApplicationPostDataHandler>();
            services.AddTransient<ILicenceApplicationViewModelBuilder, LicenceApplicationViewModelBuilder>();
            services.AddTransient<ILicenceStatusViewModelBuilder, LicenceStatusViewModelBuilder>();
            services.AddTransient<IFormDefinition, LicenceApplicationFormDefinition>();
            services.AddTransient<IFieldConfiguration, FieldConfiguration>();
            services.AddTransient<IConstantService, ConstantService>();

            // admin profile
            services.AddTransient<ILicenceRepository, LicenceRepository>();
            services.AddTransient<IAdminHomeViewModelBuilder, AdminHomeViewModelBuilder>();
            services.AddTransient<IAdminLicenceListViewModelBuilder, AdminLicenceListViewModelBuilder>();
            services.AddTransient<IAdminLicenceViewModelBuilder, AdminLicenceViewModelBuilder>();
            services.AddTransient<IAdminLicencePostDataHandler, AdminLicencePostDataHandler>();

            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(100000);
                options.Cookie.HttpOnly = true;
            });

            services.AddMvc().AddSessionStateTempDataProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("secrets/appsettings.secrets.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var logFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
                var logger = logFactory.CreateLogger("Startup Log");

                logger.LogInformation("Getting statuses from secrets");

                var defaultStatuses = GetDefaultStatuses();

                logger.LogInformation($"Got {defaultStatuses.Count} statuses from secrets");

                logger.LogInformation("Running db seed...");

                serviceScope.ServiceProvider.GetService<GLAAContext>().Seed(defaultStatuses);

                logger.LogInformation("Completed db seed");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseSession();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "securityQuestions",
                    template: "{controller}/Apply/{section}/{action}/{id}");
                routes.MapRoute(
                    name: "applicationGet",
                    template: "Licence/Apply/{controller}/{action}/{id}");
                routes.MapRoute(
                    name: "applicationPost",
                    template: "Licence/Apply/{controller}/{action}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private List<LicenceStatus> GetDefaultStatuses()
        {
            var keyValuePairs = Configuration.GetSection("DefaultStatuses")
                .AsEnumerable()
                .Where(x => x.Key.Contains("DefaultStatus") && !string.IsNullOrEmpty(x.Value))
                .ToList();

            var defaultStatuses = new List<LicenceStatus>();

            for (var i = 0; i < keyValuePairs.Count; i++)
            {
                var status = new LicenceStatus();
                Configuration.GetSection($"DefaultStatuses:{i}").Bind(status);
                if (status.InternalStatus != null) //TODO: better check for a valid status
                {
                    defaultStatuses.Add(status);
                }
            }
            return defaultStatuses;
        }
    }
}
