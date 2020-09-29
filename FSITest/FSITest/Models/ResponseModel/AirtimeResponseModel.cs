using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSITest.Models.ResponseModel
{
    public class AirtimeResponseModel
    {
        public string errorMessage { get; set; }
        public int numSent { get; set; }
        public string totalAmount { get; set; }
        public string totalDiscount { get; set; }
        public List<AirtimeResponse> responses { get; set; }
    }

    public class AirtimeResponse
    {
        public string phoneNumber { get; set; }
        public string errorMessage { get; set; }
        public string amount { get; set; }
        public string status { get; set; }
        public string requestId { get; set; }
        public string discount { get; set; }
    }

}