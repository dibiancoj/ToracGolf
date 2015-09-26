﻿using Microsoft.AspNet.Authentication.Facebook;
using Microsoft.AspNet.Authentication.Google;
using Microsoft.AspNet.Authentication.MicrosoftAccount;
using Microsoft.AspNet.Authentication.Twitter;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics.Entity;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Dnx.Runtime;
using Microsoft.Framework.Caching.Memory;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.Constants;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.Services;
using ToracGolf.Settings;
using ToracLibrary.AspNet.Caching.FactoryStore;

namespace ToracGolf
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {
            // Setup configuration sources.

            var builder = new ConfigurationBuilder(appEnv.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddJsonFile($"config.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // This reads the configuration keys from the secret store.
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Entity Framework services to the services container.
            //services.AddEntityFramework()
            //    .AddSqlServer()
            //    .AddDbContext<ApplicationDbContext>(options =>
            //        options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            //// Add Identity services to the services container.
            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();

            //grab the ef connection string
            var connectionString = Configuration["Data:DefaultConnection:ConnectionString"];

            services.AddTransient(x => new ToracGolfContext(connectionString));

            // Configure the options for the authentication middleware.
            // You can add options for Google, Twitter and other middleware as shown below.
            // For more information see http://go.microsoft.com/fwlink/?LinkID=532715
            services.Configure<FacebookAuthenticationOptions>(options =>
            {
                options.AppId = Configuration["Authentication:Facebook:AppId"];
                options.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            });

            services.Configure<MicrosoftAccountAuthenticationOptions>(options =>
            {
                options.ClientId = Configuration["Authentication:MicrosoftAccount:ClientId"];
                options.ClientSecret = Configuration["Authentication:MicrosoftAccount:ClientSecret"];
            });

            // Add MVC services to the services container.
            services.AddMvc();

            //configure the antiforgery to use the same cookie name
            services.ConfigureAntiforgery(options =>
            {
                options.CookieName = SecuritySettings.AntiforgeryCookieName;
                options.RequireSsl = false;
            });

            // Uncomment the following line to add Web API services which makes it easier to port Web API 2 controllers.
            // You will also need to add the Microsoft.AspNet.Mvc.WebApiCompatShim package to the 'dependencies' section of project.json.
            // services.AddWebApiConventions();

            //add the IMemory cache so we can add this now
            services.AddSingleton<IMemoryCache, MemoryCache>();

#if DNX451
            // utilize resource only available with .NET Framework
            //add my cached items
            var cacheFactory = new CacheFactoryStore();

            //add the state listing factory configuration
            cacheFactory.AddConfiguration(CacheKeyNames.StateListing,
                 () => MiddleLayer.States.StateListing.StateSelect(services.BuildServiceProvider().GetService<ToracGolfContext>())
                .Select(y => new SelectListItem { Text = y.Description, Value = y.StateId.ToString() })
                .ToImmutableList());

            services.AddSingleton<ICacheFactoryStore, CacheFactoryStore>((x) => cacheFactory);
#else
          //not sure if we will support .net core yet
#endif

            // Register application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.MinimumLevel = LogLevel.Information;
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            // Configure the HTTP request pipeline.

            // Add the following to the request pipeline only in development environment.
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseErrorPage();
                app.UseDatabaseErrorPage(DatabaseErrorPageOptions.ShowAll);
            }
            else
            {
                // Add Error handling middleware which catches all application specific errors and
                // sends the request to the following path or controller action.
                app.UseErrorHandler("/Home/Error");
            }

            // Add static files to the request pipeline.
            app.UseStaticFiles();

            // Add cookie-based authentication to the request pipeline.
            //app.UseIdentity();

            // Add authentication middleware to the request pipeline. You can configure options such as Id and Secret in the ConfigureServices method.
            // For more information see http://go.microsoft.com/fwlink/?LinkID=532715
            // app.UseFacebookAuthentication();
            // app.UseGoogleAuthentication();
            // app.UseMicrosoftAccountAuthentication();
            // app.UseTwitterAuthentication();

            app.UseCookieAuthentication(options =>
            {
                options.LoginPath = new Microsoft.AspNet.Http.PathString("/LogIn");
                options.AutomaticAuthentication = true;
                options.AuthenticationScheme = "Cookies";
            });

            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                // Uncomment the following line to add a route for porting Web API 2 controllers.
                // routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
            });
        }
    }
}
