using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;

using System.Xml.Linq;

namespace FscmBridgeServices.DTOS
{
    
    public class FscmFinanceOrganization
    {
        [Key]
        public string accountNumber { get; set; }
        
        public string organizationUuid { get; set; }
        public string accountName{ get; set; }
        public string currency{ get; set; }
        public string country{ get; set; }
        public string bankName{ get; set; }
        public string city{ get; set; }
        public string swiftCode{ get; set; }

    }
}
