using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TNE.Common.Logger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace WebApi1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            bool efMigrate = args.Any(arg => arg == "--ef-migrate");
            var webHost = CreateHostBuilder(args).Build();


            if (efMigrate) DoMigrate(webHost);


            webHost.Run();
        }

        static void DoMigrate(IHost webHost)
        {
            
                using (var serviceScope = webHost.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    Console.WriteLine("Applying Entity Framework migrations");
                    using (var context = serviceScope.ServiceProvider.GetService<ProductsDbContext>())
                    {

                        var pendingMigrations = context.Database.GetPendingMigrations();
                        var migrations = pendingMigrations as IList<string> ?? pendingMigrations.ToList();
                        if (!migrations.Any())
                        {
                            Console.WriteLine("No pending migratons");
                            Environment.Exit(0);
                        }

                        context.Database.Migrate();
                        Console.WriteLine("All done, closing app");
                        Environment.Exit(0);
                    }
                
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                     .UseKestrel(options => options.Listen(IPAddress.Loopback, 5100))
                     .UseTneSerilog();
                });
    }
}
