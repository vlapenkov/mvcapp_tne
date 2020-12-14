using System;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApi1;

namespace Tests
{
    /// <summary>
    /// Изымает из настроек DbContext который через БД и вставляет dbContext in Memory для более быстрого тестирования
    /// </summary>
    /// <typeparam name="TStartup"></typeparam>
    public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<ProductsDbContext>));

                services.Remove(descriptor);

                services.AddDbContext<ProductsDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                // Добавляем Authentication scheme для тестов (чтобы не запускать IS4 сервер)
                services.AddAuthentication(options => {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                            "Test", options => { });

                // используем аутентификационную схему test
                services.AddAuthorization(
               options =>
               {
                   options.AddPolicy("DefaultPolicy", policy =>
                   {
                       policy.AuthenticationSchemes.Add("Test");
                       policy.RequireAuthenticatedUser();
                        // policy.Requirements.Add(new MinimumAgeRequirement());
                    });
               }
               );
                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ProductsDbContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    db.Database.EnsureCreated();

                    
                }
            });
        }
    }
}
