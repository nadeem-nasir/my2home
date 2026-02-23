using Autofac;
using Autofac.Extensions.DependencyInjection;
using My2Home.Core.SharedKernel;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Reflection;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using My2Home.Web.Identity;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using AspNet.Security.OpenIdConnect.Primitives;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net;
using System.Threading.Tasks;
using OpenIddict.Abstractions;
using My2Home.Core.AppSettings;
using My2Home.Web.Mapper;
using My2Home.Infrastructure;
using My2Home.Infrastructure.Extensions;
namespace My2Home.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            HostingEnvironment = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

        }

        public static IConfiguration Configuration { get; set; }
        private IHostingEnvironment HostingEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //auto mapper 

            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapperProfile());
            });
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            //inject repository
            services.AddRepository();
            //sql connection 
            services.AddSingleton(c => new SqlConnectionFactory(Configuration["AppSettings:DataBaseSettings:ConnectionString"]));


            //app settings 
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            //Identity
            services.AddDbContext<ApplicationIdentityDbContext>(options =>
            {
                options.UseSqlServer(Configuration["AppSettings:DataBaseSettings:ConnectionString"]);
                options.UseOpenIddict();
            });
            services.AddIdentity<ApplicationIdentityUser, IdentityRole
                >(options =>
            {
                // User settings
                options.User.RequireUniqueEmail = true;

                //// Password settings

                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

                //    //// Lockout settings
                //    //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                //    //options.Lockout.MaxFailedAccessAttempts = 10;


            })
          .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
          .AddDefaultTokenProviders();
            // Enable Dual Authentication 

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            var secret = Configuration["AppSettings:JWTSettings:Secret"];
            var audience = Configuration["AppSettings:JWTSettings:Audience"];
            var issuer = Configuration["AppSettings:JWTSettings:Issuer"];

            //var audienceConfig = Configuration2.GetSection("AppSettings:JWTSettings");

            // Configure Identity to use the same JWT claims as OpenIddict instead
            // of the legacy WS-Federation claims it uses by default (ClaimTypes),
            // which saves you from doing the mapping in your authorization controller.
            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            });

            var keyByteArray = Encoding.UTF8.GetBytes(secret);
            var signKey = new SymmetricSecurityKey(keyByteArray);

            // Register the OpenIddict services.
            /*services.AddOpenIddict(options =>
            {
                // Register the Entity Framework stores.
                options.AddEntityFrameworkCoreStores<PMCMIdentityContext>();
                // Register the ASP.NET Core MVC binder used by OpenIddict.
                // Note: if you don't call this method, you won't be able to
                // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
                options.AddMvcBinders();
                // Enable the token endpoint.
                // Form password flow (used in username/password login requests)
                options.EnableTokenEndpoint("/connect/token");
                // Enable the authorization endpoint.
                // Form implicit flow (used in social login redirects)
                options.EnableAuthorizationEndpoint("/connect/authorize");
                // Enable the password and the refresh token flows.
                options.AllowPasswordFlow()
                       .AllowRefreshTokenFlow()
                       .AllowImplicitFlow(); // To enable external logins to authenticate

                options.SetAccessTokenLifetime(TimeSpan.FromMinutes(30));
                options.SetIdentityTokenLifetime(TimeSpan.FromMinutes(30));
                options.SetRefreshTokenLifetime(TimeSpan.FromMinutes(60));
                // During development, you can disable the HTTPS requirement.
                options.DisableHttpsRequirement();

                //options.UseRollingTokens(); //Uncomment to renew refresh tokens on every refreshToken request
                //options.AddSigningKey(new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(audienceConfig["Secret"])));

                // Note: to use JWT access tokens instead of the default
                // encrypted format, the following lines are required:
                //
                options.UseJsonWebTokens();
                options.AddSigningKey(signKey);
                options.AddEphemeralSigningKey();
            });*/


            services.AddOpenIddict()

                // Register the OpenIddict core services.
                .AddCore(options =>
                {
                    // Configure OpenIddict to use the Entity Framework Core stores and models.
                    options.UseEntityFrameworkCore()
                           .UseDbContext<ApplicationIdentityDbContext>();
                })

                // Register the OpenIddict server handler.
                .AddServer(options =>
                {
                    // Register the ASP.NET Core MVC services used by OpenIddict.
                    // Note: if you don't call this method, you won't be able to
                    // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
                    options.UseMvc();
                    
                    // Enable the authorization, logout, token and userinfo endpoints.
                    options.EnableAuthorizationEndpoint("/connect/authorize")
                           .EnableLogoutEndpoint("/connect/logout")
                           .EnableTokenEndpoint("/connect/token")
                           .EnableUserinfoEndpoint("/api/userinfo");

                    // Note: the Mvc.Client sample only uses the code flow and the password flow, but you
                    // can enable the other flows if you need to support implicit or client credentials.
                    options.AllowAuthorizationCodeFlow()
                           .AllowPasswordFlow()
                           .AllowRefreshTokenFlow()
                           .AllowImplicitFlow();

                    // Mark the "email", "profile" and "roles" scopes as supported scopes.
                    options.RegisterScopes(OpenIddictConstants.Scopes.Email,
                                           OpenIddictConstants.Scopes.Profile,
                                           OpenIddictConstants.Scopes.Roles,
                                           OpenIddictConstants.Scopes.OfflineAccess,
                                           OpenIddictConstants.Scopes.OpenId,
                                           OpenIddictConstants.Destinations.IdentityToken);

                    // When request caching is enabled, authorization and logout requests
                    // are stored in the distributed cache by OpenIddict and the user agent
                    // is redirected to the same page with a single parameter (request_id).
                    // This allows flowing large OpenID Connect requests even when using
                    // an external authentication provider like Google, Facebook or Twitter.
                    options.EnableRequestCaching();

                    // During development, you can disable the HTTPS requirement.
                    options.DisableHttpsRequirement();
                    options.SetAccessTokenLifetime(TimeSpan.FromDays(1));
                    
                    options.SetIdentityTokenLifetime(TimeSpan.FromDays(30));
                    options.SetRefreshTokenLifetime(TimeSpan.FromDays(60));

                    // Note: to use JWT access tokens instead of the default
                    // encrypted format, the following lines are required:
                    //
                    options.UseJsonWebTokens();
                    options.AddSigningKey(signKey);
                    options.AddEphemeralSigningKey();

                    // Note: if you don't want to specify a client_id when sending
                    // a token or revocation request, uncomment the following line:
                    //
                    // options.AcceptAnonymousClients();

                    //if (this.environment.IsDevelopment())
                    //{
                    //    options.DisableHttpsRequirement();
                    //    options.AddEphemeralSigningKey();
                    //}
                    options.IgnoreEndpointPermissions()
                           .IgnoreGrantTypePermissions()
                           .IgnoreScopePermissions();
                    //options.DisableScopeValidation();
                    //options.AcceptAnonymousClients();
                });

            services.AddAuthentication(options =>
            {
                // This will override default cookies authentication scheme
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
    .AddJwtBearer(options =>
    {
        options.Authority = audience;
        options.Audience = audience;
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {

            NameClaimType = OpenIdConnectConstants.Claims.Subject,
            RoleClaimType = OpenIdConnectConstants.Claims.Role,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
        };
        options.Configuration = new OpenIdConnectConfiguration();
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                if (context.Request.Path.Value.StartsWith("/api"))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                }
                return Task.CompletedTask;
            }
        };
    });
            //// https://console.developers.google.com/projectselector/apis/library?pli=1
            //.AddGoogle(options =>
            //{
            //    options.ClientId = Startup.Configuration["Authentication:Google:ClientId"];
            //    options.ClientSecret = Startup.Configuration["Authentication:Google:ClientSecret"];
            //})
            //// https://developers.facebook.com/apps
            //.AddFacebook(options =>
            //{
            //    options.AppId = Startup.Configuration["Authentication:Facebook:AppId"];
            //    options.AppSecret = Startup.Configuration["Authentication:Facebook:AppSecret"];
            //})
            //// https://apps.twitter.com/
            //.AddTwitter(options =>
            //{
            //    options.ConsumerKey = Startup.Configuration["Authentication:Twitter:ConsumerKey"];
            //    options.ConsumerSecret = Startup.Configuration["Authentication:Twitter:ConsumerSecret"];
            //})
            //// https://apps.dev.microsoft.com/?mkt=en-us#/appList
            //.AddMicrosoftAccount(options =>
            //{
            //    options.ClientId = Startup.Configuration["Authentication:Microsoft:ClientId"];
            //    options.ClientSecret = Startup.Configuration["Authentication:Microsoft:ClientSecret"];
            //});






            //.AddOAuthValidation() will throw exceptionsScheme already exists: Bearer
            //./*AddOpenIdConnect(options => { options.SaveTokens = true;});*/          
            //services.AddAuthentication()
            //  .AddCookie(cfg => cfg.SlidingExpiration = true)
            //  .AddJwtBearer(cfg =>
            //  {
            //      cfg.RequireHttpsMetadata = false;
            //      cfg.SaveToken = true;

            //      cfg.TokenValidationParameters = new TokenValidationParameters()
            //      {
            //          ValidIssuer = audienceConfig["Issuer"],
            //          ValidAudience = audienceConfig["Audience"],
            //          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(audienceConfig["Secret"]))
            //      };
            //  });


            //end identity
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            // TODO: Add DbContext and IOC
            //string dbName = Guid.NewGuid().ToString();
            //services.AddDbContext<AppDbContext>(options =>
            //    options.UseInMemoryDatabase(dbName));
            //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc()
                .AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            services.AddTransient<My2Home.Web.Services.IEmailSender, My2Home.Web.Services.EmailSender>();
           // services.AddTransient<My2Home.Web.Services.ISmsSender, My2Home.Web.Services.AuthMessageSender>();
            // return BuildDependencyInjectionProvider(services);
        }

        private static IServiceProvider BuildDependencyInjectionProvider(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            // Populate the container using the service collection
            builder.Populate(services);

            // TODO: Add Registry Classes to eliminate reference to Infrastructure
            //Assembly webAssembly = Assembly.GetExecutingAssembly();
            //Assembly coreAssembly = Assembly.GetAssembly(typeof(BaseEntity));
            //Assembly infrastructureAssembly = Assembly.GetAssembly(typeof(EfRepository)); // TODO: Move to Infrastucture Registry
            //builder.RegisterAssemblyTypes(webAssembly, coreAssembly, infrastructureAssembly).AsImplementedInterfaces();

            IContainer applicationContainer = builder.Build();
            return new AutofacServiceProvider(applicationContainer);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseCookiePolicy();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                   name: "default_route",
                   template: "{controller}/{action}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");


                // when the user types in a link handled by client side routing to the address bar 
                // or refreshes the page, that triggers the server routing. The server should pass 
                // that onto the client, so Angular can handle the route

                //routes.MapSpaFallbackRoute("spa-fallback",  new { controller = "Home", action = "Index" });
                // Catch all Route - catches anything not caught be other routes
                //routes.MapRoute(
                //    name: "catch-all",
                //    template: "{*url}",
                //    defaults: new { controller = "Home", action = "Index" });

            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501
                
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}


//https://kevinchalet.com/2018/07/02/implementing-advanced-scenarios-using-the-new-openiddict-rc3-events-model/