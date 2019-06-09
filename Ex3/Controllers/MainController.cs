//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using Ex3.Models;
//using System.IO;
//using System.Net;

//namespace Ex3.Controllers
//{
//    public class MainController : Controller
//    {
//        // GET: Main
//        public ActionResult Index()
//        {
//            return View();
//        }


//        // / display / 127.0.0.1 / 5400
//        //--------or------------
//        // / display / flight1 / 4
//        [HttpGet]
//        public ActionResult BasicDisplay(string ip, int port) {
//            IPAddress ipAd;
//            bool isIp = IPAddress.TryParse(ip, out ipAd);
//            if (isIp)
//            {
//                if ()
//                Session["IpAddress"] = ip;
//                Session["portNumber"] = port;
//                return View("BasicDisplay");
//            }
//            else
//            {                
//                //the string is actually the file's name.
//                ViewBag.fileName = ip;
//                ViewBag.returnedPath = port;
//                //need to add anything else?-------------------------------------------------------------------
//                return View("LoadAndDisplay");
//            }

//        }
//        // / display / 127.0.0.1 / 5400 / 4
//        public ActionResult AnimatedDisplay(string ip, int port, int time)
//        {
//            Session["IpAddress"] = ip;
//            Session["portNumber"] = port;
//            Session["timesPerSec"] = time;

//            return View();
//        }

//        // / save / 127.0.0.1 / 5400 / 4 / 10 / flight1
//        public ActionResult DisplayAndSave(string ip, int port, int time, int seconds, string fileName)
//        {
//            //dont know if it works
//            Session["IpAddress"] = ip;
//            Session["portNumber"] = port;
//            Session["timesPerSec"] = time;
//            Session["seconds"] = seconds;
//            ViewBag.fileName = fileName;

//            return View();
//        }


//        [HttpGet]
//        public ActionResult GetSimulatorData()
//        {
//            //need to continue this..
//            //----------------------------------------------------------------------------------------------------


//            //request data from simulator in real time and return it

//            //fill all the props (lon, lat, throttle, rudder)
//            Flight data = new Flight(); // SavingData data = simulatorConnect.getData();

//            //change before submission. this is only for testing.
//            return Json( data , JsonRequestBehavior.AllowGet);
//        }

//        [HttpPost]
//        public ActionResult SaveToFile(string fileName,List<Flight> path) {

//            //need to continue this..
//            //-------------------------------------------------------------------------------------------------------

//            return Json(HttpStatusCode.OK);
//        }

//        private string ToXml(Inforamtion flight)
//        {
//            //Initiate XML stuff
//            XmlDocument doc = new XmlDocument();
//            StringBuilder sb = new StringBuilder();
//            XmlWriterSettings settings = new XmlWriterSettings();
//            XmlWriter writer = XmlWriter.Create(sb, settings);

//            writer.WriteStartDocument();
//            writer.WriteStartElement("Flight");
//            flight.ToXml(writer);

//            writer.WriteEndElement();
//            writer.WriteEndDocument();
//            writer.Flush();
//            settings.Indent = true;

//            return sb.ToString();
//        }


//    }
//}

using Ex3.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Xml;

namespace Ex3.Controllers
{
    public class FlightController : Controller
    {
        #region Views
        // GET: Main => Action Result function - Index 
        public ActionResult Index()
        {
            return View();
        }

        // Action result function - BasicDisplay
        //        / display / 127.0.0.1 / 5400
        //        --------or------------
        //        / display / flight1 / 4
        [HttpGet]
        public ActionResult BasicDisplay(string ip, int port)
        {
            if (!validateIP(ip))
            {
                return LoadAndDisplay(ip, port);
            }
            Connect.Instance.ConnectToHost(ip, port);
            updateFlightParameters();
            Session["Lat"] = Information.Instance.Lat;
            Session["Lon"] = Information.Instance.Lon;
            return View();
        }

        // Action result function - AnimatedDisplay
        // / display / 127.0.0.1 / 5400 / 4
        [HttpGet]
        public ActionResult AnimatedDisplay(string ip, int port, int time)
        {
            Connect.Instance.ConnectToHost(ip, port);
            Information.Instance.Time = time;
            updateFlightParameters();
            Session["Lat"] = Information.Instance.Lat;
            Session["Lon"] = Information.Instance.Lon;
            Session["time"] = time;

            return View();
        }

        // Action Result function - DisplayAndSave
        // / save / 127.0.0.1 / 5400 / 4 / 10 / flight1
        public ActionResult DisplayAndSave(string ip, int port, int time, int seconds, string file)
        {
            Connect.Instance.ConnectToHost(ip, port);
            Information.Instance.Time = time;
            Information.Instance.FileName = file;
            updateFlightParameters();
            Session["Lat"] = Information.Instance.Lat;
            Session["Lon"] = Information.Instance.Lon;
            Session["time"] = time;
            Session["seconds"] = seconds;
            return View();
        }

        // Action result function - LoadAndDisplay
        [HttpGet]
        public ActionResult LoadAndDisplay(string file, int time)
        {
            string newFile = System.Web.Hosting.HostingEnvironment.MapPath(@"/App_Data/" + file);
            var logFile = System.IO.File.ReadAllLines(file);
            Information.Instance.ReadFile = new List<string>(logFile);
            Information.Instance.Index = 0;
            Session["time"] = time;
            return View(@"~\Views\Main\LoadAndDisplay.cshtml");
        }
        #endregion

        #region Useful Functions
        //getFlightInformationToFile function
        [HttpPost]
        public string getFlightInformationToFile()
        {
            updateFlightParameters();
            var flight = Information.Instance;
            string text = toXml(flight);
            text += '\n';
            if (Information.Instance.ToWrite == null)
            {
                Information.Instance.ToWrite += text;
            }
            Information.Instance.ToWrite += text;
            return text;
        }

        // getLine function
        [HttpPost]
        public string getLine()
        {
            if (Information.Instance.Index < Information.Instance.ReadFile.Count - 1)
            {
                string text = Information.Instance.ReadFile[Information.Instance.Index];
                Information.Instance.Index++;
                return text;
            }
            return "END";
        }

        // getFlightInformation functuin
        [HttpPost]
        public string getFlightInformation()
        {
            updateFlightParameters();
            var flight = Information.Instance;
            return toXml(flight);
        }

        //updateFlightParameters function
        private void updateFlightParameters()
        {
            Information.Instance.Lon = Connect.Instance.WriteData("get /position/longitude-deg\r\n");
            Information.Instance.Lat = Connect.Instance.WriteData("get /position/latitude-deg\r\n");
            Information.Instance.Rudder = Connect.Instance.WriteData("get /controls/flight/rudder\r\n");
            Information.Instance.Throttel = Connect.Instance.WriteData("get /controls/engines/engine/throttle\r\n");
        }

        //toXml function
        private string toXml(Information flight)
        {
            XmlDocument document = new XmlDocument();
            StringBuilder stringBuilder = new StringBuilder();
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(stringBuilder, xmlWriterSettings);
            writer.WriteStartDocument();
            writer.WriteStartElement("Flight");
            flight.ToXml(writer);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            xmlWriterSettings.Indent = true;
            return stringBuilder.ToString();
        }

        // saveToXML function
        [HttpPost]
        public string saveToXML()
        {
            Information.Instance.AddToXml(Information.Instance.ToWrite);
            return "saved";
        }

        public bool validateIP(string ip)
        {
            if (ip.Count(c => c == '.') != 3)
            {
                return false;
            }
            IPAddress address;
            return IPAddress.TryParse(ip, out address);
        }
        #endregion
    }
}