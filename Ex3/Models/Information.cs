using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace Ex3.Models
{
    public class Information
    {
        //members = all Information gathered in the flight simulator 
        public double Height { get; set; }
        public double Direction { get; set; }
        public double Speed { get; set; }
        public int Time { get; set; }
        public double Throttel { get; set; }
        public double Rudder { get; set; }
        public string FileName { get; set; }
        public string ToWrite { get; set; }
        public List<string> ReadFile { get; set; }
        public int Index { get; set; }
        private double oldLat = 0;
        private double oldLon = 0;
        private double lat, lon;

        #region Singleton instance of Inforamtion
        private static Information s_instace = null;
        public static Information Instance
        {
            get
            {
                if (s_instace == null)
                {
                    s_instace = new Information();
                }
                return s_instace;
            }
        }
        #endregion

        #region get & set
        //Get & Set to lat (private member) 
        public double Lat
        {
            get
            {
                return lat;
            }
            set
            {
                oldLat = lat;
                lat = value;
            }
        }
        //Get & Set to lon (private member) 
        public double Lon
        {
            get
            {
                return lon;
            }
            set
            {
                oldLon = lon;
                lon = value;
            }
        }
        #endregion

        #region Useful functions for dealing with XML
        //ToXml function 
        public void ToXml(XmlWriter xmlWriter)
        {
            xmlWriter.WriteElementString("oldLat", oldLat.ToString());
            xmlWriter.WriteElementString("oldLon", oldLon.ToString());
            xmlWriter.WriteElementString("Lat", Lat.ToString());
            xmlWriter.WriteElementString("Lon", Lon.ToString());
            xmlWriter.WriteElementString("Height", Height.ToString());
            xmlWriter.WriteElementString("Throttel", Throttel.ToString());
            xmlWriter.WriteElementString("Rudder", Rudder.ToString());
        }

        //AddToXml function
        public void AddToXml(string text)
        {
            string newText = text + Environment.NewLine;
            string newFile = System.Web.Hosting.HostingEnvironment.MapPath(@"/App_Data/" + FileName);
            string newPath = HttpContext.Current.Server.MapPath(@"/App_Data/" + FileName);
            File.WriteAllText(newPath, newText);
        }
        #endregion
    }
}