using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSITest.Models.RequestModel
{
    public class AirtimeRequestModel
    {
        public List<AirtimeRecipient> recipients { get; set; }
    }
    public class AirtimeRecipient
    {
        public string phoneNumber { get; set; }
        public string amount { get; set; }
        public string currencyCode { get; set; }
    }

    
}