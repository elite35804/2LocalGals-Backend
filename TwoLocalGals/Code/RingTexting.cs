using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RingCentral;
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

namespace Nexus
{
    public class RingTextOAuth
    {
        public string error;
        public string error_description;
        public string access_token;
        public uint expires_in;
    }

    public class RingTexting
    {
        //username: TwoLocalGalsServerSide,+18017472122,101,k9YYTceJSBqRQqumMB4sPg,r1I8MosnRlWKAn9LhzdSMAmuGaNLm4QLK8FXYafuhbqg
        //pass: #CleanFreak22

        public static async Task<string> SendSMS(string phoneNumber, string message, FranchiseStruct franchise)
        {
            try
            {
                string[] split = franchise.smsUsername.Split(',');

                var restClient = new RestClient(split[3], split[4], split[5], split[0]);
                //var restClient = new RestClient(split[3], split[4], split[0] == "production");
                await restClient.Authorize(split[1], split[2], franchise.smsPassword);

                var parameters = new CreateSMSMessage();
                parameters.from = new MessageStoreCallerInfoRequest { phoneNumber = split[1] };
                parameters.to = new MessageStoreCallerInfoRequest[] { new MessageStoreCallerInfoRequest { phoneNumber = phoneNumber } };
                parameters.text = message;

                var resp = await restClient.Restapi().Account().Extension().Sms().Post(parameters);
                Console.WriteLine("SMS sent. Message status: " + resp.messageStatus);
                return null;
            }
            catch (WebException webEx)
            {
                Database.LogThis("RingTexting.SendText(" + phoneNumber + "," + message.Length + "," + franchise.franchiseName + ")", webEx);
                return webEx.Message;
            }
            catch (Exception ex)
            {
                Database.LogThis("RingTexting.SendText(" + phoneNumber + "," + message.Length + "," + franchise.franchiseName + ")", ex);
                return ex.Message;
            }
        }

        /* static bool waitLoop = true;

       static public async Task retrieve_modify()
        {
            var restClient = new RestClient("Bp_EWf7gQ_CVF1fv7cjtFA", "8La-BYBcQkmlY1OievrweAHdCfmcvPQQW1MSTvAPCwzw", "https://platform.devtest.ringcentral.com", "TwoLocalGalsServerSide");
            await restClient.Authorize("+14242547986", "101", "F1##7sinx");

            if (restClient.token.access_token.Length > 0)
            {
                var requestParams = new ListMessagesParameters();
                requestParams.readStatus = new string[] { "Unread" };
                var resp = await restClient.Restapi().Account().Extension().MessageStore().List(requestParams);
                int count = resp.records.Length;
                Console.WriteLine(String.Format("Retrieving a list of {0} messages.", count));
                foreach (var record in resp.records)
                {
                    var messageId = record.id;
                    var updateRequest = new UpdateMessageRequest();
                    updateRequest.readStatus = "Read";
                    var result = await restClient.Restapi().Account().Extension().MessageStore(messageId.ToString()).Put(updateRequest);
                    var readStatus = result.readStatus;
                    Console.WriteLine("Message status has been changed to " + readStatus);
                    break;
                }
            }
        }

        static public async Task retrieve_delete()
        {
            var restClient = new RestClient("Bp_EWf7gQ_CVF1fv7cjtFA", "8La-BYBcQkmlY1OievrweAHdCfmcvPQQW1MSTvAPCwzw", "https://platform.devtest.ringcentral.com", "TwoLocalGalsServerSide");
            await restClient.Authorize("+14242547986", "101", "F1##7sinx");

            if (restClient.token.access_token.Length > 0)
            {
                var requestParams = new ListMessagesParameters();
                requestParams.readStatus = new string[] { "Read" };
                var resp = await restClient.Restapi().Account().Extension().MessageStore().List(requestParams);
                int count = resp.records.Length;
                Console.WriteLine(String.Format("Get get a list of {0} messages.", count));
                foreach (var record in resp.records)
                {
                    var messageId = record.id;
                    await restClient.Restapi().Account().Extension().MessageStore(messageId).Delete();
                    Console.WriteLine(String.Format("Message {0} has been deleted.", messageId));
                }
            }
        }

        static public async Task receive_reply()
        {
            var restClient = new RestClient("Bp_EWf7gQ_CVF1fv7cjtFA", "8La-BYBcQkmlY1OievrweAHdCfmcvPQQW1MSTvAPCwzw", "https://platform.devtest.ringcentral.com", "TwoLocalGalsServerSide");
            await restClient.Authorize("+14242547986", "101", "F1##7sinx");

            if (restClient.token.access_token.Length > 0)
            {
                try
                {
                    var eventFilters = new[]
                    {
                        "/restapi/v1.0/account/~/extension/~/message-store/instant?type=SMS",
                        "/restapi/v1.0/account/~/extension/~/voicemail",
                    };
                    var subscription = new RingCentral.Paths.Restapi.Subscription.Renew(restClient, eventFilters, message =>
                    {
                        reply_sms_message(restClient, message);
                    });
                    var subscriptionInfo = await subscription.Subscribe();
                    Console.WriteLine("Waiting for notifications ...");
                    while (waitLoop)
                    {
                        Thread.Sleep(1000);
                    }
                    await subscription.Revoke();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        static private void reply_sms_message(RestClient rc, String message)
        {
            dynamic jsonObj = JObject.Parse(message);
            string eventType = jsonObj["event"];
            if (eventType.Contains("/message-store/instant"))
            {
                String senderNumber = jsonObj["body"]["from"]["phoneNumber"];
                Console.WriteLine("Recieved message from: " + senderNumber);
                var parameters = new CreateSMSMessage();
                parameters.from = new MessageStoreCallerInfoRequest { phoneNumber = "+14242547986" };
                parameters.to = new MessageStoreCallerInfoRequest[] { new MessageStoreCallerInfoRequest { phoneNumber = senderNumber } };
                parameters.text = "This is an automatic reply. Thank you for your message!";
                var resp = rc.Restapi().Account().Extension().Sms().Post(parameters);
                Console.WriteLine("Replied message sent.");
                waitLoop = false;
            }
            else if (eventType.Contains("/voicemail"))
            {
                String senderNumber = jsonObj["body"]["from"]["phoneNumber"];
                if (senderNumber != null)
                {
                    Console.WriteLine("Recieved a voicemail from: " + senderNumber);
                    var parameters = new CreateSMSMessage();
                    parameters.from = new MessageStoreCallerInfoRequest { phoneNumber = "+14242547986" };
                    parameters.to = new MessageStoreCallerInfoRequest[] { new MessageStoreCallerInfoRequest { phoneNumber = senderNumber } };
                    parameters.text = "This is an automatic reply. Thank you for your voice message! I will get back to you asap.";
                    var resp = rc.Restapi().Account().Extension().Sms().Post(parameters);
                    Console.WriteLine("Replied message sent.");
                    waitLoop = false;
                }
                else
                {
                    Console.WriteLine("Private call, no phone number to reply!");
                }
            }
            else
            {
                Console.WriteLine("Not an event we are waiting for.");
            }
        }*/
    }
}