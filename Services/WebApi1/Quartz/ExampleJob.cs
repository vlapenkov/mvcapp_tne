using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading.Tasks;

namespace WebApi1
{
    public class ExampleJob : IJob
    {
        IServiceScopeFactory _factory;
        private ILogger _logger;

        public ExampleJob(ILogger<ExampleJob> logger
            )
        {
            _logger = logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
          //  Console.WriteLine($"Logger {DateTime.Now}");
       
           _logger.LogInformation($"Logger {DateTime.Now}");
        }
    }
}