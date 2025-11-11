using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Collections.Generic;

namespace FscmBridgeServices.DTOS
{
    
     public class ContractParticipantResponse
        {
            public List<ContractParticipant> Content { get; set; }
    
        }
        public class ContractParticipant
        {
            public string Uuid { get; set; }
         
        }

}
