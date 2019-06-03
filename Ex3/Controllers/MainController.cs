using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ex3.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BasicDisplay(string ip, int port, int? time) {


            return View();
        }

        [HttpGet]
        public ActionResult GetSimulatorData()
        {


            return Json( new { lon = 200,lat = 200 }, JsonRequestBehavior.AllowGet);
        }

    }
}