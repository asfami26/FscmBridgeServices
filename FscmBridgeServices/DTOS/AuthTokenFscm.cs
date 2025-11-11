using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;

using System.Xml.Linq;
using System.Collections.Generic;

namespace FscmBridgeServices.DTOS
{
    public class AuthTokenFscm
    {
        public string Access_Token { get; set; }
        public string Tenant_Id { get; set; }
        public string Refresh_Token { get; set; }
        public string Sub { get; set; }
        public List<string> Aud { get; set; }
        public string User_Uuid { get; set; }
        public string Scope { get; set; }
        public string Iss { get; set; }
        public string Token_Type { get; set; }
        public int Expires_In { get; set; }
    }
}
