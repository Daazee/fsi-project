using FSITest.BLL;
using FSITest.Models;
using FSITest.Models.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace FSITest.Controllers
{
    public class SandBoxController : ApiController
    {

        [ActionName("SendSmsMessage")]
        [HttpPost]
        public async Task<HttpResponseMessage> SendSmsMessage([FromBody] SmsRequest request)
        {
            GenericApiResponse<bool> rsp = new GenericApiResponse<bool>();
            try
            {
                ApiCalls api = new ApiCalls();
                bool ApiResp = await api.SendSms(request);
                rsp.data = ApiResp;
                rsp.message = "Successful";
                rsp.response = "00";
                return Request.CreateResponse(HttpStatusCode.BadRequest, rsp);
            }
            catch (Exception ex)
            {
                rsp.data = false;
                rsp.message = "failed";
                rsp.response = "99";
                rsp.responsedata = ex.ToString();
                return Request.CreateResponse(HttpStatusCode.BadRequest, rsp);
            }
        }
    }
}