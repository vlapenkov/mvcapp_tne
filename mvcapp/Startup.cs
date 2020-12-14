using IdentityModel.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using mvc.services;
using Refit;
using Shared;

using System;
using WebApi1.Contracts.Interfaces;

namespace mvcapp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        private void CheckSameSite(HttpContext httpContext, CookieOptions options)
        {
            if (options.SameSite == SameSiteMode.None)
            {
                //var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
                //if (MyUserAgentDetectionLib.DisallowsSameSiteNone(userAgent))
                {
                    options.SameSite = SameSiteMode.Unspecified;
                }
            }
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // нужен для нормальной работы OpenIdConnect в Chrome 8 без https.
            //Также нужен код в IS4 
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
                options.OnAppendCookie = cookieContext =>
                    CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
                options.OnDeleteCookie = cookieContext =>
                    CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
            });


            services.AddHttpContextAccessor();
         
            services.AddTransient<MyAccessTokenHandler>();
            //  services.AddTransient<AccessTokenDelegatingHandler>();
            //  JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            #region identity
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    // чтобы User.Identity.Name работало
                  options.TokenValidationParameters.NameClaimType = "name";
                    options.SignInScheme = "Cookies";

                    options.Authority = "http://local_host:5500";
                    options.RequireHttpsMetadata = false;

                    options.ClientId = "mvc";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code id_token";

                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.Scope.Add("Api1.Scope");
                    options.Scope.Add("Api2.Scope");

                    options.Scope.Add("phone");
                    options.Scope.Add("custom.profile");

                    options.Scope.Add("offline_access");
                });

            #endregion 
            services.AddControllersWithViews()              
                ; 

           var baseAddress= new Uri(Configuration["Services:WebApi1"]);
            services.AddRefitClient<IProductService>().ConfigureHttpClient(c => c.BaseAddress = baseAddress)
                .AddHttpMessageHandler<MyAccessTokenHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // нужен для нормальной работы OpenIdConnect в Chrome 8 без https.
            //Также нужен код в IS4 

            app.UseCookiePolicy();
          // app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });
           


            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
