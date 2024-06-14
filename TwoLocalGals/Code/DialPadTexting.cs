using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using RestSharp;

namespace Nexus
{
    public class DialPadTexting
    {
        //userid: it's per user setup in DialPad
        //apikey: BxprTez7M6euCJPNx8HJCwZJsWcXfJUUYCTVT8hzXkuLwGnpwYhNGuZmUUHKraG8yb8kDVBudPAHwLz8cqGCqeKqYeG256Lf7yX6

        //franchise smsUsername will be used to hold the DialPad User_ID and DialPad Api-key (token) seperated by a comma
        public static async Task<string> SendSMS(string phoneNumber, string message, FranchiseStruct franchise)
        {
            try
            {
                const SecurityProtocolType tls13 = (SecurityProtocolType)12288;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //| tls13;

                string[] userIdAndOfficeId = franchise.smsUsername.Split(','); //userId first element, officeId is second, type is third i.e. 123456789,987654321,office
                var restClient = new RestClient("https://dialpad.com/api/v2/sms");
                var request = new RestRequest();
                request.Method = Method.Post;
                request.AddHeader("Authorization", $"Bearer {franchise.smsPassword.Trim()}");
                //,\"sender_group_id\":6064735801081856,\"sender_group_type\":\"office\"
                request.AddStringBody($"{{\"infer_country_code\":true,\"to_numbers\":[\"{phoneNumber}\"],\"text\":\"{message}\",\"user_id\":{userIdAndOfficeId[0].Trim()},\"sender_group_id\":{userIdAndOfficeId[1].Trim()},\"sender_group_type\":{userIdAndOfficeId[2].ToLower().Trim()}}}", DataFormat.Json);
                //request.AddStringBody("{\"user_id\": \"6614048293814272\",\"text\": \"test\",\"infer_country_code\": true,\"to_numbers\": [\"8018158217\"] }", DataFormat.Json);
                //request.AddJsonBody(new { user_id = franchise.smsUsername, text = message, to_numbers = new List<string>{phoneNumber} });
                var response = await restClient.PostAsync(request);

                //Console.WriteLine("SMS sent. Message status: " + response.Content);
                if (response.StatusCode != HttpStatusCode.OK)
                    return response.Content;

                return null;
            }
            catch (WebException webEx)
            {
                Database.LogThis("DialPadTexting.SendText(" + phoneNumber + "," + message.Length + "," + franchise.franchiseName + ")", webEx);
                return webEx.Message;
            }
            catch (Exception ex)
            {
                Database.LogThis("DialPadTexting.SendText(" + phoneNumber + "," + message.Length + "," + franchise.franchiseName + ")", ex);
                return ex.Message;
            }
        }
    }
}