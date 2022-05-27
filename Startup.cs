using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QNA.IdentityServer.Data;
using QNA.IdentityServer.Extensions;
using QNA.IdentityServer.Filters;
using QNA.IdentityServer.Helpers;
using QNA.IdentityServer.Models;
using QNA.IdentityServer.Providers;
using QNA.IdentityServer.Services;
using System;

namespace QNA.IdentityServer
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddOptions();
            //services.Configure<ConfigSettings>(Configuration.GetSection("ApplicationSettings"));
            // services.AddMvc();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.Secure = CookieSecurePolicy.Always;
            });
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.Tokens.ProviderMap.Add(AccountOptions.EmailProvider,
                new TokenProviderDescriptor(
               typeof(CustomEmailConfirmationTokenProvider<ApplicationUser>)));
                options.Tokens.EmailConfirmationTokenProvider = AccountOptions.EmailProvider;
                options.Tokens.PasswordResetTokenProvider = AccountOptions.EmailProvider; 
                options.Password.RequiredLength = 8;
                //lock for 1 year, basically we need to lock him indefinitely until he resets his password
                options.Lockout.DefaultLockoutTimeSpan = new System.TimeSpan(365, 0, 0, 0);
                options.Lockout.MaxFailedAccessAttempts = Convert.ToInt32(Configuration.GetSection("MaxFailedLoginAttemptsAllowed").Value);


            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddUserStore<ApplicationUserStore>()
            .AddUserManager<ApplicationUserManager>();

            //lifespan for email confirmation token:
            services.AddTransient<CustomEmailConfirmationTokenProvider<ApplicationUser>>();
            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(HtmlEncode));
            });
            // configure identity server with in-memory stores, keys, clients and scopes
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryPersistedGrants()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients(Configuration))
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<ProfileService>()
                .AddDeveloperSigningCredential();
            //.AddTemporarySigningCredential();

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 443;
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); 
            //services.AddTransient<IProfileService, ProfileService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) //, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole(LogLevel.Trace);
            if (env.IsDevelopment())
            //if (true)
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseCookiePolicy();
            app.UseSession();
            //  app.UseMvc();
            // app.UseAuthentication(); // not needed, since UseIdentityServer adds the authentication middleware
            app.UseIdentityServer();
            app.UseStaticFiles();
             app.UseMvcWithDefaultRoute();
            app.UseHttpsRedirection();
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Account}/{action=Login}");
            //});
        }
    }
}
