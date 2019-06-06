using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ex3.Models;
using System.IO;
using System.Net;

namespace Ex3.Controllers
{
    public class MainController : Controller
    {
        //not good, dont do that:
        //public SavingData data = new SavingData();
    
        // GET: Main
        public ActionResult Index()
        {
            return View();
        }


        // / display / 127.0.0.1 / 5400
        //--------or------------
        // / display / flight1 / 4
        public ActionResult BasicDisplay(string ip, int port) {
            IPAddress ipAd;
            bool isIp = IPAddress.TryParse(ip, out ipAd);
            if (isIp)
            {
                Session["IpAddress"] = ip;
                Session["portNumber"] = port;
                return View("BasicDisplay");
            }
            else
            {                
                //the string is actually the file's name.
                ViewBag.fileName = ip;
                ViewBag.returnedPath = port;
                //need to add anything else?-------------------------------------------------------------------
                return View("LoadAndDisplay");
            }
            
        }
        // / display / 127.0.0.1 / 5400 / 4
        public ActionResult AnimatedDisplay(string ip, int port, int time)
        {
            Session["IpAddress"] = ip;
            Session["portNumber"] = port;
            Session["timesPerSec"] = time;

            return View();
        }

        // / save / 127.0.0.1 / 5400 / 4 / 10 / flight1
        public ActionResult DisplayAndSave(string ip, int port, int time, int seconds, string fileName)
        {
            //dont know if it works
            Session["IpAddress"] = ip;
            Session["portNumber"] = port;
            Session["timesPerSec"] = time;
            Session["seconds"] = seconds;
            ViewBag.fileName = fileName;

            return View();
        }


        [HttpGet]
        public ActionResult GetSimulatorData()
        {
            //need to continue this..
            //----------------------------------------------------------------------------------------------------

      
            //request data from simulator in real time and return it

            //fill all the props (lon, lat, throttle, rudder)
            SavingData data = new SavingData(); // SavingData data = simulatorClient.getData();

            //change before submission. this is only for testing.
            return Json( data , JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveToFile(string fileName,List<SavingData> path) {

            //need to continue this..
            //-------------------------------------------------------------------------------------------------------

            return Json(HttpStatusCode.OK);
        }


    }
}