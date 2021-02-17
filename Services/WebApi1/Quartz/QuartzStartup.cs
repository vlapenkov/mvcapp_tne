using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi1.Quartz
{
    public class QuartzStartup
    {
        IServiceProvider _serviceProvider;
        public QuartzStartup(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Start()
        {
            
            var scheduler =await StdSchedulerFactory.GetDefaultScheduler();
            // other code is same

            scheduler.JobFactory = (IJobFactory)_serviceProvider.GetService(typeof(IJobFactory));

            await scheduler.Start();
            var sampleJob = JobBuilder.Create<ExampleJob>().Build();
            // var sampleTrigger = TriggerBuilder.Create().StartNow().WithCronSchedule("0 0/1 * * * ?").Build();
            var sampleTrigger = TriggerBuilder.Create().StartNow().WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromSeconds(10)).RepeatForever()).Build();
            await scheduler.ScheduleJob(sampleJob, sampleTrigger);
        }

        public async Task Stop()
        { 
        
        }
    }
}
