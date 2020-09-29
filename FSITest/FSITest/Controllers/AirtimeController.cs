using FSITest.BLL;
using FSITest.Models;
using FSITest.Models.RequestModel;
using FSITest.Models.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FSITest.Controllers
{
    public class AirtimeController : Controller
    {
        // GET: Airtime
        [HttpGet]
        public ActionResult SendAirtime()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> SendAirtime(AirtimeRecipient request)
        {
            var result = new AirtimeResponseModel();
            try
            {
                ApiCalls api = new ApiCalls();



                var requestObject = new AirtimeRequestModel();

                var recipient = new AirtimeRecipient();
                var recipients = new List<AirtimeRecipient>();
                recipient.amount = request.amount;
                recipient.currencyCode = "NGN";
                recipient.phoneNumber = request.phoneNumber;

                requestObject.recipients = new List<AirtimeRecipient>();

                requestObject.recipients.Add(recipient);


                result = await api.SendAirtme(requestObject);

                if (result != null)
                {
                    SmsRequest smsRequest = new SmsRequest();
                    smsRequest.to = request.phoneNumber;
                    smsRequest.from = "FSI";
                    smsRequest.message = $"Your recharge of {request.amount} airtime is successful";
                    
                    var resp = api.SendSms(smsRequest);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}