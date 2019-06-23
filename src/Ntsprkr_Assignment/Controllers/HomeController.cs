using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ntsprkr_Assignment.Bussiness;
using Ntsprkr_Assignment.Data.Entities;
using Ntsprkr_Assignment.Models;
using Ntsprkr_Assignment.NotificationService;

namespace Ntsprkr_Assignment.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HealthCheckBussiness _healthCheckBussiness;

        public HomeController(ILogger<HomeController> logger,HealthCheckBussiness healthCheckBussiness)
        {
            _logger = logger;
            _healthCheckBussiness = healthCheckBussiness;
        }

        public IActionResult Index()
        {
            var apps = _healthCheckBussiness.GetAll().ToList();
            return View(apps);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new TargetAppVM());
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(TargetAppVM targetApp)
        {
            if (ModelState.IsValid)
            {
                _healthCheckBussiness.AddNew(targetApp.ConvertToEntity());
                return RedirectToAction("Index");
            }
            return View(targetApp);
        }
        [HttpGet]
        public IActionResult Edit(string id)
        {

            var targetApp = new TargetAppVM();
            targetApp.ParseFromEntity(_healthCheckBussiness.GetById(id));
            return View(targetApp);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TargetAppVM targetApp)
        {

            if (ModelState.IsValid)
            {
                _healthCheckBussiness.Update(targetApp.ConvertToEntity());
                return RedirectToAction("Index");
            }
            return View(targetApp);
        }
        public IActionResult Details(string id)
        {
            var targetApp = new TargetAppVM();
            targetApp.ParseFromEntity(_healthCheckBussiness.GetById(id));
            return View(targetApp);
        }
        public IActionResult Delete(string id)
        {
            var targetApp = _healthCheckBussiness.GetById(id);
            _healthCheckBussiness.Delete(targetApp);
            return View();
        }

        public IActionResult Monitoring()
        {
            return View();
        }

        public List<AppMonitoringVM> GetApps()
        {
            return _healthCheckBussiness.GetAll().Select(x => new AppMonitoringVM() { id = x.Id.ToString(), name = x.Name, state = x.IsDown ? "DOWN":"UP" }).ToList();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
