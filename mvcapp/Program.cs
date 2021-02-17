using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using TNE.Common.Logger;

namespace mvcapp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            int x = 1;
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>

                {
                    webBuilder
                         .UseStartup<Startup>().UseTneSerilog();
                    // .UseKestrel(options => options.Listen(IPAddress.Loopback, 5000));                   

                });
           

            return hostBuilder;
        }
    }
}
