using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Ntsprkr_Assignment.Data;
using Ntsprkr_Assignment.Models;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Ntsprkr_Assignment.Extensions
{
    public static class QuartzExtensions
    {
        public static void AddQuartz(this IServiceCollection services, Type jobType)
        {
            services.Add(new ServiceDescriptor(typeof(IJob), jobType, ServiceLifetime.Transient));
            services.AddSingleton<IJobFactory, ScheduledJobFactory>();
            services.AddSingleton<IJobDetail>(provider =>
            {
                return JobBuilder.Create<ScheduledJob>()
                  .WithIdentity("Sample.job", "group1")
                  .Build();
            });


            services.AddSingleton<IScheduler>(provider =>
            {
                var schedulerFactory = new StdSchedulerFactory();
                var scheduler = schedulerFactory.GetScheduler().Result;
                scheduler.JobFactory = provider.GetService<IJobFactory>();
                scheduler.Start();
                return scheduler;
            });

        }

        public static void UseQuartz(this IApplicationBuilder app)
        {
            var props = new NameValueCollection
            {
                { "quartz.serializer.type", "binary" }
            };
            var sched = app.ApplicationServices.GetService<IScheduler>();
            sched.Start();

            var db = app.ApplicationServices.CreateScope().ServiceProvider.GetService<ApplicationDbContext>();
            foreach (var targetApp in db.TargetApps.ToList())
            {
                sched.AddHealthCheckJob(targetApp.Id.ToString(), targetApp.Interval);
            }
        }

        public static void AddHealthCheckJob(this IScheduler scheduler, string appId,int interval)
        {
            var job = JobBuilder.Create<ScheduledJob>()
                    .WithIdentity(appId, "group1")
                    .Build();
            var trigger = TriggerBuilder.Create()
            .WithIdentity(appId, "group1")
            .StartNow()
            .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(interval)
                .RepeatForever())
            .ForJob(job)
            .Build();
            scheduler.ScheduleJob(job, trigger);
        }
    }
}
