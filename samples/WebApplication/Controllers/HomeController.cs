using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FeatureToggle;
using WebApplication.FeatureToggles;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = Is<FrontpageUIFeature>.Enabled ? "FrontPageFeature is enabled in Azure Table Storage !!!" : "Getting started";

            ViewBag.ComingSoonMessage = Is<ComingSoonFeature>.Enabled ? "Coming soon Feature is now available" : "Feature is coming soon";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = Is<AboutPageFeature>.Enabled ? "AboutPageFeature is enabled in DocumentDB !!!" : "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = Is<RetiringSoonFeature>.Enabled ? "Feature is going away" : "Feature is now gone";

            return View();
        }
    }
}