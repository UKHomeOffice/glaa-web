using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GLAA.Domain;
using GLAA.Domain.Models;
using GLAA.Repository;
using GLAA.Services;
using GLAA.Services.Admin;
using GLAA.Services.Automapper;
using GLAA.Services.LicenceApplication;
using GLAA.Web.Core.Services;
using GLAA.Web.FormLogic;
using GLAA.Web.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace GLAA.Web
{
    public class Startup
    {
        private static readonly string[] FirstNames = { "Aaron", "Abdul", "Abe", "Abel", "Abraham", "Adam", "Adan", "Adrian", "Abby", "Abigail", "Adele", "Christina", "Doug", "Chantelle", "Adam", "Luke", "Conrad", "Moray" };
        private static readonly string[] LastNames = { "Abbott", "Acosta", "Adams", "Adkins", "Aguilar", "Aguilara", "McDonald", "MacDonald", "Danson", "Spokes", "Grinnell", "Jackson" };

        private static readonly GLAARole[] Roles =
        {
            new GLAARole
            {
                Name = "Administrator",
                Description = "A role for administrators"
            },
            new GLAARole
            {
                Name = "Labour Provider",
                Description = "A role for labour providers"
            },
            new GLAARole
            {
                Name = "Labour User",
                Description = "A role for labour users"
            },
            new GLAARole
            {
                Name = "OGD User",
                Description = "A role for Other Government Department users"
            },
        };

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var user = Environment.GetEnvironmentVariable("APP_USER");
            var password = Environment.GetEnvironmentVariable("APP_PASS");
            var server = Environment.GetEnvironmentVariable("DB_SERVER");
            var port = Environment.GetEnvironmentVariable("DB_PORT");

            //TODO: take DataSource intro environment
            var connectionString = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == EnvironmentName.Development ?
                Configuration["GLAAContext"] :
                $"Data Source={server},{port};Initial Catalog=GLAA_Core;Integrated Security=False;User Id={user};Password={password};MultipleActiveResultSets=True";

            services.AddDbContext<GLAAContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<SignInManager<GLAAUser>, SignInManager<GLAAUser>>();

            services.AddIdentity<GLAAUser, GLAARole>()
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
            services.AddTransient<IAdminUserListViewModelBuilder, AdminUserListViewModelBuilder>();
            services.AddTransient<IAdminUserViewModelBuilder, AdminUserViewModelBuilder>();
            services.AddTransient<IAdminUserPostDataHandler, AdminUserPostDataHandler>();

            // notify
            services.AddTransient<IEmailService>(x => new EmailService(
                services.BuildServiceProvider().GetService<ILoggerFactory>(),
                Configuration.GetSection("GOVNotify")["APIKEY"]));

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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
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
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "applicationPost",
                    template: "Licence/Apply/{controller}/{action}");
            });
            BuildRoles(serviceProvider).Wait();
            BuildAdminUser(serviceProvider).Wait();
            BuildUsers(serviceProvider).Wait();
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

        private static async Task BuildRoles(IServiceProvider serviceProvider)
        {
            var rm = serviceProvider.GetRequiredService<RoleManager<GLAARole>>();

            foreach (var role in Roles)
            {
                var exists = await rm.RoleExistsAsync(role.Name);

                if (!exists)
                {
                    await rm.CreateAsync(new GLAARole(role.Name, role.Description));
                }
            }
        }

        private async Task BuildAdminUser(IServiceProvider serviceProvider)
        {
            var um = serviceProvider.GetRequiredService<UserManager<GLAAUser>>();

            var su = await um.FindByEmailAsync(Configuration.GetSection("SuperUser")["SuperUserEmail"]);

            if (su == null)
            {
                su = new GLAAUser
                {
                    UserName = Configuration.GetSection("SuperUser")["SuperUserEmail"],
                    Email = Configuration.GetSection("SuperUser")["SuperUserEmail"]
                };
                var result = await um.CreateAsync(su, Configuration.GetSection("SuperUser")["SuperUserPassword"]);

                if (result.Succeeded)
                {
                    su = await um.FindByEmailAsync(Configuration.GetSection("SuperUser")["SuperUserEmail"]);
                } else {
                    throw new Exception($"Could not create superuser {result.Errors.First().Description}");
                }
            }

            if (!await um.IsInRoleAsync(su, "Administrator"))
            {
                await um.AddToRoleAsync(su, "Administrator");
            }
        }

        private static async Task BuildUsers(IServiceProvider serviceProvider)
        {
            var um = serviceProvider.GetRequiredService<UserManager<GLAAUser>>();

            if (um.Users.Count() <= 1)
            {
                var rnd = new Random();

                for (var i = 0; i < 50; i++)
                {
                    string un;
                    string fn;
                    do
                    {
                        var f = FirstNames[rnd.Next(FirstNames.Length)];
                        var l = LastNames[rnd.Next(LastNames.Length)];
                        un = $"{f}.{l}@example.com";
                        fn = $"{f} {l}";
                    } while (await um.FindByEmailAsync(un) != null);

                    var user = new GLAAUser
                    {
                        UserName = un,
                        Email = un,
                        FullName = fn
                    };
                    var pw = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

                    var createResult = await um.CreateAsync(user, pw);

                    if (createResult.Succeeded)
                    {
                        user = await um.FindByEmailAsync(un);
                        var availableNames = Roles.Select(r => r.Name).ToArray();
                        var role = availableNames[rnd.Next(availableNames.Length)];

                        await um.AddToRoleAsync(user, role);
                    }
                }
            }
        }
    }
}
