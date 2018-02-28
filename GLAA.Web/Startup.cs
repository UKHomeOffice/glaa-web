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
using Amazon.S3;
using Amazon.Runtime;
using GLAA.Services.AccountCreation;
using GLAA.Services.PublicRegister;
using GLAA.Scheduler.Tasks;
using GLAA.Scheduler.Scheduling;
using GLAA.Services.Extensions;

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

            //builder.AddEnvironmentVariables();

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
            services.AddScoped<ISessionHelper, SessionHelper>();

            // automapper
            services.AddSingleton<IMapper>(x => new AutoMapperConfig().Configure().CreateMapper());

            // http session
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // account creation
            services.AddTransient<IAccountCreationPostDataHandler, AccountCreationPostDataHandler>();
            services.AddTransient<IAccountCreationViewModelBuilder, AccountCreationViewModelBuilder>();

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
            services.AddScoped<IReferenceDataProvider, ReferenceDataProvider>();

            // admin profile
            services.AddTransient<ILicenceRepository, LicenceRepository>();
            services.AddTransient<IAdminHomeViewModelBuilder, AdminHomeViewModelBuilder>();
            services.AddTransient<IAdminLicenceListViewModelBuilder, AdminLicenceListViewModelBuilder>();
            services.AddTransient<IAdminLicencePostDataHandler, AdminLicencePostDataHandler>();
            services.AddTransient<IAdminUserListViewModelBuilder, AdminUserListViewModelBuilder>();
            services.AddTransient<IAdminUserViewModelBuilder, AdminUserViewModelBuilder>();
            services.AddTransient<IAdminUserPostDataHandler, AdminUserPostDataHandler>();
            services.AddTransient<IAdminStatusRecordsViewModelBuilder, AdminStatusRecordsViewModelBuilder>();

            // public register
            services.AddTransient<IPublicRegisterViewModelBuilder, PublicRegisterViewModelBuilder>();
            services.AddTransient<IPublicRegisterPostDataHandler, PublicRegisterPostDataHandler>();

            services.AddTransient<IFileUploadService, FileUploadService>();

            // notify
            services.AddTransient<IEmailService>(x => new EmailService(
                services.BuildServiceProvider().GetService<ILogger<EmailService>>(),
                Configuration.GetSection("GOVNotify")["APIKEY"]));

            // scheduled tasks
            services.AddSingleton<IScheduledTask, SendTestEmailTask>();
            services.AddScheduler((sender, args) =>
            {
                Console.Write(args.Exception.Message);
                args.SetObserved();
            });

            // replace the default logged with a timed logger
            services.Replace(ServiceDescriptor.Singleton(typeof(ILogger<>), typeof(TimedLogger<>)));

            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(100000);
                options.Cookie.HttpOnly = true;
            });

            services.AddMvc().AddSessionStateTempDataProvider();

            ConfigureAWS(services);
        }

        private void ConfigureAWS(IServiceCollection services)
        {
            var opts = Configuration.GetAWSOptions();

            opts.Credentials = new EnvironmentVariablesAWSCredentials();

            services.AddDefaultAWSOptions(opts);

            services.AddAWSService<IAmazonS3>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("secrets/appsettings.secrets.json", optional: true);

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
                app.UseDatabaseErrorPage();
            }
            else
            {
                //app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/Home/Error");
            }

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var logger = app.ApplicationServices.GetRequiredService<ILogger<Startup>>();                

                logger.TimedLog(LogLevel.Information, "Getting statuses from secrets");

                var defaultStatuses = GetDefaultStatuses();

                logger.TimedLog(LogLevel.Information, $"Got {defaultStatuses.Count} statuses from secrets");

                logger.TimedLog(LogLevel.Information, "Running db seed...");

                var dbContext = serviceScope.ServiceProvider.GetService<GLAAContext>();

                dbContext.Seed(defaultStatuses);

                BuildRoles(serviceProvider).Wait();
                BuildSuperUser(serviceProvider).Wait();
                BuildUsers(serviceProvider).Wait();
                var adminUsers = BuildAdminUsersAsync(serviceProvider).Result;

                dbContext.AddAdminUsersWithFullLicence(adminUsers);

                logger.TimedLog(LogLevel.Information, "Completed db seed");
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

        private async Task BuildSuperUser(IServiceProvider serviceProvider)
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
                }
                else
                {
                    throw new Exception($"Could not create superuser {result.Errors.First().Description}");
                }
            }

            if (!await um.IsInRoleAsync(su, "Administrator"))
            {
                await um.AddToRoleAsync(su, "Administrator");
            }
        }

        private async Task<IEnumerable<GLAAUser>> BuildAdminUsersAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<GLAAUser>>();

            var keyValuePairs = Configuration.GetSection("AdminUsers")
                .AsEnumerable()
                .Where(x => x.Key.Contains("AdminUsers") 
                    && x.Key.Contains("Email") 
                    && !string.IsNullOrEmpty(x.Value))
                .ToList();

            var adminUsers = new List<GLAAUser>();

            for (var i = 0; i < keyValuePairs.Count; i++)
            {
                var user = new GLAAUser();

                Configuration.GetSection($"AdminUsers:{i}").Bind(user);
                var password = Configuration.GetSection($"AdminUsers:{i}:Password").Value;
                var email = Configuration.GetSection($"AdminUsers:{i}:Email").Value;

                user.UserName = email;

                adminUsers.Add(user);

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    if (!await userManager.IsInRoleAsync(user, "Administrator"))
                    {
                        await userManager.AddToRoleAsync(user, "Administrator");
                    }
                }
            }

            return adminUsers;
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
                    string f;
                    string l;
                    do
                    {
                        f = FirstNames[rnd.Next(FirstNames.Length)];
                        l = LastNames[rnd.Next(LastNames.Length)];
                        un = $"{f}.{l}@example.com";
                    } while (await um.FindByEmailAsync(un) != null);

                    var user = new GLAAUser
                    {
                        UserName = un,
                        Email = un,
                        FirstName = f,
                        LastName = l
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
