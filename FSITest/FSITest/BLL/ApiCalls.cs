using FSITest.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
    }
}


            
           
               
               
