using System.Collections.Generic;

namespace FscmBridgeServices.Repository.Entity
{
    public class FscmContract
    {
        public string? programUuid { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public bool isDeactivated { get; set; }
        public bool financingAfterCutOffTime { get; set; }
        public List<Funder>? funder { get; set; }
        public List<Buyer>? buyer { get; set; }
        public List<Seller>? seller { get; set; }
        public List<OptionRate>? optionRates { get; set; }
        public Suspension? suspensions { get; set; }
        public bool isAutoRetrySettlement { get; set; }
    }
}
