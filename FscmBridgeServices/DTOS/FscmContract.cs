using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Collections.Generic;

namespace FscmBridgeServices.DTOS
{
       public class FscmContract
        {
            public string programUuid { get; set; }
            public string? name { get; set; }
            public string description { get; set; }
            public bool isDeactivated { get; set; }
            public bool financingAfterCutOffTime { get; set; }
            public List<Funder> funder { get; set; }
            public List<Buyer> buyer { get; set; }
            public List<Seller> seller { get; set; }
            public List<OptionRate> optionRates { get; set; }
            public Suspension suspensions { get; set; }
            public bool isAutoRetrySettlement { get; set; }
        }

        public class Funder
        {
            public string organizationUuid { get; set; }
            public string organizationName { get; set; }
            public string code { get; set; }
            public string type { get; set; }
        }

        public class Buyer
        {
            public string organizationUuid { get; set; }
            public string organizationName { get; set; }
            public string code { get; set; }
            public string type { get; set; }
            public bool isPrincipal { get; set; }
        }

        public class Seller
        {
            public string organizationUuid { get; set; }
            public string organizationName { get; set; }
            public string code { get; set; }
            public string type { get; set; }
            public bool isPrincipal { get; set; }
        }

        

        

        public class OptionRate
        {
            public string currency { get; set; }
            public int divisor { get; set; }
        }

        public class Suspension
        {
            public bool isForever { get; set; }
            public bool isSpesificDate { get; set; }
            public string overdueDays { get; set; }
            public string overdueAmount { get; set; }
            public bool allowFinance { get; set; }
            public bool fromInvoice { get; set; }
            public string spesificDateTime { get; set; }
            public int emailNotificationBefore { get; set; }
            public int emailNotificationAfter { get; set; }
            public int participantsOverdueAmount { get; set; }
            public string creditRating { get; set; }
            public int quoteRestrictionDays { get; set; }
        }

}
