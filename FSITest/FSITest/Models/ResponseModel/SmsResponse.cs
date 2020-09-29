using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSITest.Models.ResponseModel
{
   
        public class SmsResponse
        {
            public Smsmessagedata SMSMessageData { get; set; }
        }

        public class Smsmessagedata
        {
            public string Message { get; set; }
            public Recipient[] Recipients { get; set; }
        }

        public class Recipient
        {
            public int statusCode { get; set; }
            public string number { get; set; }
            public string cost { get; set; }
            public string status { get; set; }
            public string messageId { get; set; }
        }

}