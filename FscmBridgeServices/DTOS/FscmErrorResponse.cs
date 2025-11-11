using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;

using System.Xml.Linq;

namespace FscmBridgeServices.DTOS
{
    public class FscmErrorResponse
    {
        public class Default
        {
            public string code { get; set; }
            public object message { get; set; }
        }

        public class Errors
        {
            public Default @default { get; set; }
        }

        public class Root
        {
            public Errors errors { get; set; }
        }


    }
    public class InvalidToken
    {
        public string error { get; set; }
    }

}
