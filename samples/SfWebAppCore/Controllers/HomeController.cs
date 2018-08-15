using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FeatureToggle;
using Microsoft.AspNetCore.Mvc;
using SfWebAppCore.FeatureToggles;
using SfWebAppCore.Models;

namespace SfWebAppCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["ComingSoonMessage"] = Is<ComingSoonFeature>.Enabled ? "Coming soon Feature is now available" : "Feature is coming soon";

            ViewData["LimitedPeriodMessage"] = Is<LimitedTimeFeature>.Enabled ? "Feature only available for limited time" : "Limited Time Feature is not available";
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = Is<CoolNewFeatureToggle>.Enabled ? "AboutPageFeature is enabled in Service Fabric Config !!!" : "Your application description page."; 

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = Is<RetiringSoonFeature>.Enabled ? "Feature is going away" : "Feature is now gone";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
