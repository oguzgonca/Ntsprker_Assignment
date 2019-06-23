using Microsoft.EntityFrameworkCore;
using Ntsprkr_Assignment.Data;
using Ntsprkr_Assignment.Data.Entities;
using Ntsprkr_Assignment.Extensions;
using Ntsprkr_Assignment.NotificationService;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ntsprkr_Assignment.Bussiness
{
    public class HealthCheckBussiness
    {
        private readonly NotificationProvider _notificationProvider;
        private IScheduler _scheduler;
        private ApplicationDbContext _app;
        public HealthCheckBussiness(NotificationProvider notificationProvider, ApplicationDbContext appcontext, IScheduler scheduler)
        {
            _notificationProvider = notificationProvider;
            _scheduler = scheduler;
            _app = appcontext;
        }
        public void AddNew(TargetApp targetApp)
        {
            targetApp.Id = Guid.NewGuid();
            _app.Set<TargetApp>().Add(targetApp);
            _app.SaveChanges();
            _scheduler.AddHealthCheckJob(targetApp.Id.ToString(), targetApp.Interval);

        }
        public void Update(TargetApp targetApp)
        {
            _app.Entry<TargetApp>(targetApp).State = EntityState.Modified;
            var old = GetById(targetApp.Id.ToString());
            if (targetApp.Interval != old.Interval)
            {
                _scheduler.UnscheduleJob(new TriggerKey(targetApp.Id.ToString()));
                _scheduler.AddHealthCheckJob(targetApp.Id.ToString(), targetApp.Interval);
            }
            if (targetApp.IsDown && !old.IsDown)
            { 
                _notificationProvider.NotifyAsync(NotifyTypes.Email, $"<strong>{targetApp.Name}</strong> health check endpoint returned status code different from 2##");
            }
            _app.SaveChanges();
        }
        public void Delete(TargetApp targetApp)
        {
            _app.Entry<TargetApp>(targetApp).State = EntityState.Deleted;
            _scheduler.UnscheduleJob(new TriggerKey(targetApp.Id.ToString()));
            _app.SaveChanges();
        }
        public IQueryable<TargetApp> GetAll()
        {
            return _app.Set<TargetApp>().AsNoTracking();
        }
        public TargetApp GetById(string Id)
        {
            return _app.Set<TargetApp>().AsNoTracking().SingleOrDefault(x => x.Id.ToString() == Id);
        }
    }
}
