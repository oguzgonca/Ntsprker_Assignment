using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ntsprkr_Assignment.Bussiness;
using Ntsprkr_Assignment.Data;
using Ntsprkr_Assignment.NotificationService;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ntsprkr_Assignment.Models
{
    public class ScheduledJob : IJob
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<ScheduledJob> logger;
        private readonly IServiceProvider serviceProvider;


        public ScheduledJob(IConfiguration configuration, ILogger<ScheduledJob> logger,IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.serviceProvider = serviceProvider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var provider = this.serviceProvider.CreateScope().ServiceProvider;
            var app = provider.GetService<HealthCheckBussiness>().GetById(context.Trigger.Key.Name);
            var result = provider.GetService<IHealthCheckService>().HealthCheckAsync(app.HealthCheckUrl, x => x.IsSuccessStatusCode).Result;

            if (!result && !app.IsDown)
            {
                app.IsDown = true;
                provider.GetService<HealthCheckBussiness>().Update(app);
            }
            else if (result && app.IsDown)
            {
                app.IsDown = false;
                provider.GetService<HealthCheckBussiness>().Update(app);
            }
            await Task.CompletedTask;

        }
    }
}
