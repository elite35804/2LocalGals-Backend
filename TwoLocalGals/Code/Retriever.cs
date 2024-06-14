using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Web;

namespace Nexus
{
    public class Retriever
    {
        const string url = @"https://retrieveronline.transactiongateway.com/api/transact.php";
        const string username = @"demo";
        const string password = @"password";

        #region IsRetrieverAccount
        public static bool IsRetrieverAccount(string value)
        {
            try
            {
                return value.StartsWith("[RT]");
            }
            catch { }
            return false;
        }
        #endregion

        #region Charge
        public static string Charge(string username, string password, string cardNumber, string expMo, string expYear, string address, string zip, string ccv, decimal amount, bool refund, bool auth, out string invoice, out string transID)
        {
            transID = null;
            invoice = null;
            try
            {
                if (expMo.Length == 1) expMo = "0" + expMo;
                if (expYear.Length == 4) expYear = expYear.Substring(2);

                string ret = "Unknown Error";
                for (int i = 0; i < 3; i++)
                {
                    if (i != 0) Thread.Sleep(1000);

                    Dictionary<string, string> values = new Dictionary<string, string>();

                    values.Add("type", auth ? "auth" : (refund ? "credit" : "sale"));
                    values.Add("username", username);
                    values.Add("password", password);
                    values.Add("address1", address);
                    values.Add("zip", zip);
                    values.Add("ccexp", expMo + expYear);
                    values.Add("ccnumber", cardNumber);
                    values.Add("ccv", ccv);
                    values.Add("amount", amount.ToString());

                    Dictionary<string, string> result = Process(values);

                    if (result["response"] == "1")
                    {
                        transID = result["transactionid"];
                        return null;
                    }
                    ret = result["responsetext"];
                }
                return ret;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        public static string Capture(string username, string password, string authTransID, decimal amount, out string transID)
        {
            transID = null;
            try
            {
                string ret = "Unknown Error";
                for (int i = 0; i < 3; i++)
                {
                    if (i != 0) Thread.Sleep(1000);

                    Dictionary<string, string> values = new Dictionary<string, string>();

                    values.Add("type", "capture");
                    values.Add("username", username);
                    values.Add("password", password);
                    values.Add("transactionid", authTransID);
                    values.Add("amount", amount.ToString());

                    Dictionary<string, string> result = Process(values);

                    if (result["response"] == "1")
                    {
                        transID = result["transactionid"];
                        return null;
                    }
                    ret = result["responsetext"];
                }
                return ret;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #region Void
        public static string Void(string username, string password, string transID, out string voidTransID)
        {
            voidTransID = null;
            try
            {
                string ret = "Unknown Error";
                for (int i = 0; i < 3; i++)
                {
                    if (i != 0) Thread.Sleep(1000);

                    Dictionary<string, string> values = new Dictionary<string, string>();

                    values.Add("type", "void");
                    values.Add("username", username);
                    values.Add("password", password);
                    values.Add("transactionid", transID);

                    Dictionary<string, string> result = Process(values);

                    if (result["response"] == "1")
                    {
                        voidTransID = result["transactionid"];
                        return null;
                    }
                    ret = result["responsetext"];
                }
                return ret;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region Process
        private static Dictionary<string, string> Process(Dictionary<string, string> values)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                WebRequest request = WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Timeout = 180000;


                List<string> content = new List<string>();
                foreach (string key in values.Keys)
                    content.Add(HttpUtility.UrlEncode(key) + "=" + HttpUtility.UrlEncode(values[key]));

                using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(string.Join("&", content));
                }

                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        
                        Dictionary<string, string> ret = new Dictionary<string, string>();
                        foreach (string value in streamReader.ReadToEnd().Split('&'))
                        {
                            string[] split = value.Split('=');
                            ret.Add(split[0], split[1]);
                        }
                        return ret;
                    }
                }
            }
            catch (Exception ex)
            {
                Dictionary<string, string> ret = new Dictionary<string, string>();
                ret.Add("response", "0");
                ret.Add("responsetext", ex.Message);
                return ret;
            }
        }
        #endregion
    }
}