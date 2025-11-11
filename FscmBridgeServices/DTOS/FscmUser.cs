using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace ELOBWU.DTO
{
    public class FscmUser
    {

        public string username { get; set; } 
        public string firstName { get; set; } 
        public string country { get; set; } 
        public string city { get; set; } 
        public string mobileNumber { get; set; } 
        public string address1 { get; set; } 
        public string addressName { get; set; } 
        public string email { get; set; }
        public bool otp { get; set; } 
        public string idType { get; set; }
        public string jobTitle { get; set; } 
        public string postalCode { get; set; } 
        public string idNumber { get; set; } 
        public string timeZoneId { get; set; } 
    }
}
