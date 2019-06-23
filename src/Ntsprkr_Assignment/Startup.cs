using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ntsprkr_Assignment.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using Ntsprkr_Assignment.NotificationService;
using System.Collections.Specialized;
using Quartz.Impl;
using Ntsprkr_Assignment.Models;
using Quartz;
using Ntsprkr_Assignment.Bussiness;
using Ntsprkr_Assignment.Extensions;

namespace Ntsprkr_Assignment
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<NotificationProvider>(provider => new NotificationProvider()
            {
                Notifiers = new List<INotifier>() { new MailNotifier(
                    new MailNotifierOptions() {
                        SmtpServer = "smtp.gmail.com",
                        From = "YourMailAddress",
                        To = new List<string>(){"To Mail Address"},
                        Subject = "Notification:Application Is Down",
                        BodyTemplate = @"Notification from HealthChecker <br /><br />{{notification}} <br /><br />FYI",
                        Port=587,
                        UserName="YourUserName",
                        Password = "YourPassword"
                    }),
                    new SMSNotifer()
                }
            });
            services.AddScoped<IHealthCheckService, HealthCheckService>();
            services.AddScoped<HealthCheckBussiness>();
            services.AddQuartz(typeof(ScheduledJob));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [Obsolete]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();
            env.ConfigureNLog("nlog.config");
            //add NLog to ASP.NET Core
            loggerFactory.AddNLog();
            //app.UseQuartz();
            //add NLog.Web
            app.AddNLogWeb();
            app.UseQuartz();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            
        }
    }
}
