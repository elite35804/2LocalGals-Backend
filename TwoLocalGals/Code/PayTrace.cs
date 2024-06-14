using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Concurrent;

namespace Nexus
{
    public class PayTrace
    {
        public class PayTraceOAuth
        {
            public string error;
            public string error_description;
            public string access_token;
            public uint expires_in;
            public uint created_at;
        }

        public class PayTraceResult
        {
            public bool success;
            public int response_code;
            public string status_message;
            public int transaction_id;
            public Dictionary<string, string[]> errors;
        }

        public static ConcurrentDictionary<string, string> tokens = new ConcurrentDictionary<string, string>();

        #region IsPayTraceAccount
        public static bool IsPayTraceAccount(string value)
        {
            try
            {
                return value.Contains("@");
            }
            catch { }
            return false;
        }
        #endregion

        #region Charge
        public static string Charge(string username, string password, string cardNumber, string expMo, string expYear, string address, string zip, string ccv, decimal amount, out string invoice, out string transID)
        {
            transID = null;
            invoice = null;
            try
            {
                object content = new
                {
                    amount = amount,
                    credit_card = new
                    {
                        number = cardNumber,
                        expiration_month = expMo,
                        expiration_year = expYear
                    },
                    csc = ccv,
                    billing_address = new
                    {
                        street_address = address,
                        zip = zip
                    }
                };

                PayTraceResult result = Process("https://api.paytrace.com/v1/transactions/sale/keyed", content, username, password);
                if (!result.success)
                {
                    if (result.errors != null && result.errors.Count > 0)
                    {
                        List<string> errors = new List<string>();
                        foreach(var item in result.errors)
                        {
                            foreach(var error in item.Value)
                            {
                                errors.Add(error);
                            }
                        }
                        return string.Join(", ", errors);
                    }
                    return "Code(" + result.response_code + ") - " + result.status_message;
                }
                else
                {
                    transID = result.transaction_id.ToString();
                    return null;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region Void
        public static string Void(string username, string password, string transID, out string voidTransID)
        {
            voidTransID = null;
            try
            {
                object content = new
                {
                    transaction_id = transID
                };

                PayTraceResult result = Process("https://api.paytrace.com/v1/transactions/void", content, username, password);
                if (!result.success)
                {
                    return "Code(" + result.response_code + ") - " + result.status_message;
                }
                else
                {
                    voidTransID = result.transaction_id.ToString();
                    return null;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region Refund
        public static string Refund(string username, string password, string cardNumber, string expMo, string expYear, string address, string zip, string ccv, decimal amount, out string invoice, out string transID)
        {
            invoice = null;
            transID = null;
            try
            {
                object content = new
                {
                    amount = amount,
                    credit_card = new
                    {
                        number = cardNumber,
                        expiration_month = expMo,
                        expiration_year = expYear
                    },
                    csc = ccv,
                    billing_address = new
                    {
                        street_address = address,
                        zip = zip
                    }
                };

                PayTraceResult result = Process("https://api.paytrace.com/v1/transactions/refund/keyed", content, username, password);
                if (!result.success)
                {
                    return "Code(" + result.response_code + ") - " + result.status_message;
                }
                else
                {
                    transID = result.transaction_id.ToString();
                    return null;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region Authenicate
        private static PayTraceOAuth Authenicate(string username, string password)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                WebRequest request = WebRequest.Create("https://api.paytrace.com/oauth/token");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Timeout = 180000;

                using (Stream requestStream = request.GetRequestStream())
                {
                    string param = string.Format("grant_type=password&username={0}&password={1}", username, password);
                    byte[] reqeustBuff = Encoding.UTF8.GetBytes(param);
                    requestStream.Write(reqeustBuff, 0, reqeustBuff.Length);
                }

                return JsonRequest<PayTraceOAuth>(request);
       
            }
            catch (Exception ex)
            {
                return new PayTraceOAuth { error = "Exception", error_description = ex.Message };
            }
        }
        #endregion

        #region Process
        private static PayTraceResult Process(string url, object content, string username, string password)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                PayTraceOAuth auth = Authenicate(username, password);
                if (auth.error != null)
                {
                    return new PayTraceResult { response_code = -2, status_message = auth.error + ": " + (auth.error_description ?? "") };
                }
                else
                {
                    WebRequest request = WebRequest.Create(url);
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.Timeout = 180000;
                    ((HttpWebRequest)request).ProtocolVersion = HttpVersion.Version11;
                    ((HttpWebRequest)request).Headers[HttpRequestHeader.Authorization] = "Bearer " + auth.access_token;

                    using (Stream requestStream = request.GetRequestStream())
                    {
                        string json = JsonConvert.SerializeObject(content);
                        byte[] reqeustBuff = Encoding.UTF8.GetBytes(json);
                        requestStream.Write(reqeustBuff, 0, reqeustBuff.Length);
                    }

                    return JsonRequest<PayTraceResult>(request);
                }
            }
            catch (Exception ex)
            {
                return new PayTraceResult { response_code = -1, status_message = "EX: " + ex.Message };
            }
        }
        #endregion

        #region JsonRequest
        private static T JsonRequest<T>(WebRequest request)
        {
            string body = "";
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        body = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)ex.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            body = reader.ReadToEnd();
                        }
                    }
                }
            }
            return JsonConvert.DeserializeObject<T>(body);
        }
        #endregion
    }
}