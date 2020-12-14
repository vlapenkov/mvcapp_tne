using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using mvcapp;
using System;
using System.Collections.Generic;
using System.Text;
using WebApi1;

namespace Tests
{
    public class AppTestFixture<TStartup> : WebApplicationFactory<Startup>
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            var builder = Host.CreateDefaultBuilder().ConfigureWebHostDefaults(webBuilder =>
            { webBuilder.UseStartup<Startup>().UseTestServer(); 
            
            
            }

            ).ConfigureAppConfiguration((hostingContext, config) => {
                config.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: false);
            });
            return builder;
        }



    }
}
