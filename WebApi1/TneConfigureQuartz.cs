using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi1
{
    public static class TneConfigureQuartz
    {
        public static void ConfigureQuartz(this IServiceCollection services)
        {
            services.AddQuartz((q) =>
            {
                // handy when part of cluster or you want to otherwise identify multiple schedulers
                // q.SchedulerId = "Scheduler-Core";

                // we take this from appsettings.json, just show it's possible
                // q.SchedulerName = "Quartz ASP.NET Core Sample Scheduler";

                // we could leave DI configuration intact and then jobs need to have public no-arg constructor
                // the MS DI is expected to produce transient job instances
                // this WONT'T work with scoped services like EF Core's DbContext
                q.UseMicrosoftDependencyInjectionJobFactory(options =>
                {
                    // if we don't have the job in DI, allow fallback to configure via default constructor
                    options.AllowDefaultConstructor = true;
                });

                // or for scoped service support like EF Core DbContext
                // q.UseMicrosoftDependencyInjectionScopedJobFactory();

                // these are the defaults
                q.UseSimpleTypeLoader();
                //q.UseInMemoryStore();
                //q.UseDefaultThreadPool(tp =>
                //{
                //    tp.MaxConcurrency = 10;
                //});

                q.AddJob<ExampleJob>(j => j
                    .StoreDurably() // we need to store durably if no trigger is associated
                                    // .WithDescription("my awesome job")
                    .WithIdentity("ExampleJob")
                );


                //q.AddTrigger(t => t              
                //    .ForJob("ExampleJob")
                //    .StartNow() //0 15 10 ? * *
                //    .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromSeconds(10)).RepeatForever())                
                //);

                q.AddTrigger(t => t    
    .WithCronSchedule("0 42 14 ? * *")
    .ForJob("ExampleJob"));

            }
            );
            services.AddQuartzServer(options =>
            {
                options.WaitForJobsToComplete = true;
            });
        }
    }
}
