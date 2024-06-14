using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using System.Threading;

namespace Nexus
{
    class ECreditCard
    {
        /*public static string ePNAccount = "0613449";
        public static string restrictKey = "36n075Vw22pP0xC";*/

        #region Charge
        public static string Charge(string ePNAccount, string restrictKey, string cardNumber, string expMo, string expYear, string address, string zip, decimal amount, out string invoice, out string transID)
        {
            transID = null;
            invoice = null;
            try
            {
                string[] split = null;
                for (int i = 0; i < 3; i++)
                {
                    if (i != 0) Thread.Sleep(1000);

                    List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();

                    values.Add(new KeyValuePair<string, string>("TranType", "Sale"));
                    values.Add(new KeyValuePair<string, string>("ePNAccount", ePNAccount));
                    values.Add(new KeyValuePair<string, string>("RestrictKey", restrictKey));
                    values.Add(new KeyValuePair<string, string>("Inv", "report"));
                    values.Add(new KeyValuePair<string, string>("HTML", "No"));
                    values.Add(new KeyValuePair<string, string>("CVV2Type", "0"));
                    values.Add(new KeyValuePair<string, string>("CVV2", ""));

                    values.Add(new KeyValuePair<string, string>("CardNo", cardNumber));
                    values.Add(new KeyValuePair<string, string>("ExpMonth", expMo));
                    values.Add(new KeyValuePair<string, string>("ExpYear", expYear));
                    values.Add(new KeyValuePair<string, string>("Address", address));
                    values.Add(new KeyValuePair<string, string>("Zip", zip));
                    values.Add(new KeyValuePair<string, string>("Total", amount.ToString()));

                    split = Process(@"https://www.eprocessingnetwork.com/cgi-bin/tdbe/transact.pl", values);

                    if (split.Length >= 5)
                    {
                        invoice = split[3];
                        transID = split[4];
                    }
                    if (split[0].StartsWith("Y")) return null;
                    if (!split[0].Contains("remote server")) break;
                }
                return split[0].Substring(1);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region Void
        public static string Void(string ePNAccount, string restrictKey, string transID, out string voidTransID)
        {
            voidTransID = null;
            try
            {
                List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();

                values.Add(new KeyValuePair<string, string>("TranType", "Void"));
                values.Add(new KeyValuePair<string, string>("ePNAccount", ePNAccount));
                values.Add(new KeyValuePair<string, string>("RestrictKey", restrictKey));
                values.Add(new KeyValuePair<string, string>("HTML", "No"));
                values.Add(new KeyValuePair<string, string>("Inv", "report"));
                values.Add(new KeyValuePair<string, string>("TransID", transID));

                string[] split = Process(@"https://www.eprocessingnetwork.com/cgi-bin/tdbe/transact.pl", values);

                if (split.Length >= 5) voidTransID = split[4];
                if (split[0].StartsWith("Y")) return null;
                return split[0].Substring(1);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region Refund
        public static string Refund(string ePNAccount, string restrictKey, string cardNumber, string expMo, string expYear, string address, string zip, decimal amount, out string invoice, out string transID)
        {
            transID = null;
            invoice = null;
            try
            {
                List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();

                values.Add(new KeyValuePair<string, string>("TranType", "Return"));
                values.Add(new KeyValuePair<string, string>("ePNAccount", ePNAccount));
                values.Add(new KeyValuePair<string, string>("RestrictKey", restrictKey));
                values.Add(new KeyValuePair<string, string>("Inv", "report"));
                values.Add(new KeyValuePair<string, string>("HTML", "No"));

                values.Add(new KeyValuePair<string, string>("CardNo", cardNumber));
                values.Add(new KeyValuePair<string, string>("ExpMonth", expMo));
                values.Add(new KeyValuePair<string, string>("ExpYear", expYear));
                values.Add(new KeyValuePair<string, string>("Address", address));
                values.Add(new KeyValuePair<string, string>("Zip", zip));
                values.Add(new KeyValuePair<string, string>("Total", amount.ToString()));

                string[] split = Process(@"https://www.eprocessingnetwork.com/cgi-bin/tdbe/transact.pl", values);

                if (split.Length >= 5)
                {
                    invoice = split[3];
                    transID = split[4];
                }
                if (split[0].StartsWith("Y")) return null;
                return split[0].Substring(1);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region CloseBatch
        public static string CloseBatch(string ePNAccount, string restrictKey)
        {
            try
            {
                List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();

                values.Add(new KeyValuePair<string, string>("TranType", "CloseBatch"));
                values.Add(new KeyValuePair<string, string>("ePNAccount", ePNAccount));
                values.Add(new KeyValuePair<string, string>("RestrictKey", restrictKey));
                values.Add(new KeyValuePair<string, string>("HTML", "No"));

                string[] split = Process(@"https://www.eprocessingnetwork.com/cgi-bin/tdbe/transact.pl", values);

                if (split[0].StartsWith("Y")) return null;
                return split[0].Substring(1);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region Process
        private static string[] Process(string url, List<KeyValuePair<string, string>> values)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                WebRequest request = WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Timeout = 180000;

                string param = "";
                for (int i = 0; i < values.Count; i++)
                {
                    if (i > 0) param += "&";
                    param += (HttpUtility.UrlEncode(values[i].Key) + "=" +  HttpUtility.UrlEncode(values[i].Value));
                }

                using (Stream requestStream = request.GetRequestStream())
                {
                    byte[] reqeustBuff = ASCIIEncoding.ASCII.GetBytes(param);
                    requestStream.Write(reqeustBuff, 0, reqeustBuff.Length);
                }

                using (WebResponse response = request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        string responseString = "";
                        byte[] buff = new byte[4096];
                        int bytesRead = int.MaxValue;
                        while (bytesRead > 0)
                        {
                            bytesRead = responseStream.Read(buff, 0, buff.Length);
                            responseString += ASCIIEncoding.ASCII.GetString(buff, 0, bytesRead);
                        }
                        return responseString.Replace("\"", "").Split(',');
                    }
                }
            }
            catch (Exception ex)
            {
                return new string[] { "Error Processing: " + ex.Message };
            }
        }
        #endregion
    }
}
