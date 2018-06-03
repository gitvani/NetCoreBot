using System.IO;
using System.Net;
using ChatBotWithCSharp.Models;
using Newtonsoft.Json;

namespace ChatBotWithCSharp
{
    public class NLP
    {
        public string GetReply(string message)
        {
            string httpResponse = string.Empty;
            string url = @"https://api.wit.ai/message?v=20180521&q=" + message;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Authorization", "Bearer 7S5KXBOGAGYNLTANRBAUSWQTEHOSXOIS");

            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                httpResponse = reader.ReadToEnd();
            }
            WitAiResponse witAiResponse = JsonConvert.DeserializeObject<WitAiResponse>(httpResponse);
            var reply = _convertWitAiResponseToReply(witAiResponse);

            return reply;

        }


        private string _convertWitAiResponseToReply(WitAiResponse witAiResponse)
        {
            if (witAiResponse.Entities.Intent == null)
            {
                return "Tôi không hiểu ý bạn cho lắm.";
            }

            string intent = witAiResponse.Entities.Intent[0].Value;
           
            if(intent == "greeting")
            {
                return "Xin chào, tôi là Chatbot. Tôi giúp gì được cho bạn?";
            }
            else if (intent == "price")
            {
                string productName = "";
                if(witAiResponse.Entities.Service != null)
                {
                    productName = "dịch vụ "  + witAiResponse.Entities.Service[0].Value;
                }

                return string.Format("Giá của {0} là {1}", productName, 100000);
            }

            return "Chào";
        }
    }
}
