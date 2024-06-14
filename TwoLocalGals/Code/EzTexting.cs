using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Xml;

namespace Nexus
{
    public class EzTexting
    {
        /* https://www.eztexting.com/developers/
           User: 2localgals Pass: 2localgals** */

        public static string SendText(string phoneNumber, string message, FranchiseStruct franchise)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                WebRequest webRequest = WebRequest.Create("https://app.eztexting.com/sending/messages?format=xml");
                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded";

                using (Stream stream = webRequest.GetRequestStream())
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(@"User=" + franchise.smsUsername + @"&Password=" + franchise.smsPassword + @"&PhoneNumbers[]=" + phoneNumber + "&Message=" + message);
                    stream.Write(buffer, 0, buffer.Length);
                }
                using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.Created)
                    {
                        Database.LogThis("EzTexting.SendText(" + phoneNumber + "," + message.Length + "," + franchise.smsUsername + "): " + response.StatusCode, null);
                        return "Error Code: " + response.StatusCode + ", " + response.StatusDescription;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Database.LogThis("EzTexting.SendText(" + phoneNumber + "," + message.Length + "," + franchise.smsUsername + ")", ex);
                return ex.Message;
            }
        }

    }
}