using FSITest.Models;
using FSITest.Models.RequestModel;
using FSITest.Models.ResponseModel;
using Newtonsoft.Json;
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
        public async Task<bool> SendSms(SmsRequest requestObj)
        {           
            string BaseUrl = ConfigurationManager.AppSettings["FsiUrl"].ToString();
            string ApiKey = ConfigurationManager.AppSettings["ApiKey"].ToString();
            BaseUrl += "/atlabs/messaging";
            string contentType = "application/json";
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("Sandbox-Key", "6fee89c336c046c4f0473d004906e3a7");

                    client.DefaultRequestHeaders
                        .Accept
                        .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

                    client.DefaultRequestHeaders.Add("Accept", contentType);

                    var data = new StringContent(JsonConvert.SerializeObject(requestObj));
                    var response = await client.PostAsync(BaseUrl, data);


                   // string result = response.Content.ReadAsStringAsync().Result;
                    if(response.IsSuccessStatusCode)
                        return true;
                    else
                        return false;

                    //responseModel = JsonConvert.DeserializeObject<AirtimeResponseModel>(result);

                    //return responseModel;
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
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Sandbox-Key", "6fee89c336c046c4f0473d004906e3a7");

                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

                client.DefaultRequestHeaders.Add("Accept", contentType);

                var data = new StringContent(JsonConvert.SerializeObject(requestObject));
                var response = await client.PostAsync(url, data);

                string result = response.Content.ReadAsStringAsync().Result;

                responseModel = JsonConvert.DeserializeObject<AirtimeResponseModel>(result);

                return responseModel;
            }
        }

    }
}


            
           
               
               
