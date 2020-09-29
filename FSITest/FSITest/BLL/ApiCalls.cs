using FSITest.Models;
using FSITest.Models.RequestModel;
using FSITest.Models.ResponseModel;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace FSITest.BLL
{
    public class ApiCalls
    {
        public bool SendSms(SmsRequest requestObj)
        {
            IRestResponse APiResp = null;
            var request = new RestRequest(Method.POST);
            string BaseUrl = ConfigurationManager.AppSettings["FsiUrl"].ToString();
            string ApiKey = ConfigurationManager.AppSettings["ApiKey"].ToString();
            BaseUrl += "/atlabs/messaging";
            try
            {
                request.AddHeader("ContentType", "application/json"); 

                request.AddHeader("Sandbox-Key", ApiKey);
                request.RequestFormat = DataFormat.Json;
                var client = new RestSharp.RestClient(BaseUrl);

                request.AddJsonBody(requestObj);
                APiResp = client.Execute(request);

                if (APiResp.IsSuccessful)
                {
                    //parse response
                    return true;
                }
                else
                {
                    return false;

                }
            }
            catch (Exception ex)
            {
                //todo log exception
                return false;
            }
        }

        public async Task<AirtimeResponseModel> SendAirtme(AirtimeRequestModel requestObject)
        {
            AirtimeResponseModel responseModel = new AirtimeResponseModel();
            string baseUrl = "https://sandboxapi.fsi.ng";
            string sendAirtime = "/atlabs/airtime/send";
            string url = baseUrl + sendAirtime;

            string contentType = "application/json";

            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("https://sandboxapi.fsi.ng/nibss/bvnr");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Sandbox-Key", "6fee89c336c046c4f0473d004906e3a7");

                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

                client.DefaultRequestHeaders.Add("Accept", contentType);

                //var requestObject = new AirtimeRequestModel();

                //var recipient = new AirtimeRecipient();
                //var recipients = new List<AirtimeRecipient>();
                //recipient.amount = "100";
                //recipient.currencyCode = "NGN";
                //recipient.phoneNumber = "+2347036135901";

                //requestObject.recipients = new List<AirtimeRecipient>();

                //requestObject.recipients.Add(recipient);
                var data = new StringContent(JsonConvert.SerializeObject(requestObject));
                var response = await client.PostAsync(url, data);

                string result = response.Content.ReadAsStringAsync().Result;

                responseModel = JsonConvert.DeserializeObject<AirtimeResponseModel>(result);

                return responseModel;
            }
        }

    }
}


            
           
               
               
