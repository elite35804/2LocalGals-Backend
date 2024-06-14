using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Xml;

namespace Nexus
{
    public class GoogleMaps
    {
        private static object lookupLock = new object();
        private static Dictionary<string, DrivingRoute> lookupTable = Database.GetDrivingRoutes();

        public static void Test()
        {
            //GetRoute("2468 S Davis Blvd", "84010", "111 S 300 E", "84010");
        }

        #region GetDrivingRoute
        public static DrivingRoute GetDrivingRoute(string srcAddr, string dstAddr)
        {
            try
            {
                string lookupKey = srcAddr + "|" + dstAddr;

                if (!string.IsNullOrEmpty(srcAddr) && !string.IsNullOrEmpty(dstAddr))
                {
                    lock (lookupLock)
                    {
                        if (lookupTable.ContainsKey(lookupKey))
                        {
                            return lookupTable[lookupKey];
                        }
                    }

                    string url = @"https://maps.googleapis.com/maps/api/distancematrix/xml?key=AIzaSyDFdxggzKvZHQFlTiTDs-p87IWX0YYeJ3U&units=imperial";
                    url = Globals.BuildQueryString(url, "origins", srcAddr);
                    url = Globals.BuildQueryString(url, "destinations", dstAddr);

                    WebRequest webRequest = WebRequest.Create(url);
                    webRequest.Method = "GET";

                    using(HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            using (Stream responseStream = response.GetResponseStream())
                            {
                                DrivingRoute route = new DrivingRoute();
                                XmlTextReader reader = new XmlTextReader(responseStream);

                                if (reader.ReadToDescendant("duration"))
                                {
                                    if (reader.ReadToDescendant("value"))
                                    {
                                        reader.Read();
                                        route.travelTime = decimal.Parse(reader.Value);

                                        if (reader.ReadToFollowing("distance"))
                                        {
                                            if (reader.ReadToDescendant("value"))
                                            {
                                                reader.Read();
                                                route.distance = decimal.Parse(reader.Value) * 0.000621371m;

                                                lock (lookupLock)
                                                {
                                                    lookupTable.Add(lookupKey, route);
                                                }
                                                Database.AddDrivingRoute(lookupKey, route.distance, route.travelTime);
                                                return route;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch { }
            return new DrivingRoute();
        }
        #endregion

        #region GetDrivingLink
        public static string GetDrivingLink(string[] addrData)
        {
            string url = @"http://maps.google.com/maps?f=d&t=h";
            try
            {
                for (int i = 0; i < addrData.Length; i++)
                {
                    if (i == 0) url = Globals.BuildQueryString(url, "saddr", addrData[i]);
                    else if (i == 1) url = Globals.BuildQueryString(url, "daddr", addrData[i]);
                    else url += ("+to:" + HttpUtility.UrlEncode(addrData[i]));
                }
            }
            catch { }
            return url;
        }
        #endregion
    }
}