using System.Collections.Generic;

namespace FscmBridgeServices.Common.DTOS
{
    public class FscmContractDto
    {
        public string? ProgramUuid { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsDeactivated { get; set; }
        public bool FinancingAfterCutOffTime { get; set; }
        public List<FunderDto>? Funder { get; set; }
        public List<BuyerDto>? Buyer { get; set; }
        public List<SellerDto>? Seller { get; set; }
        public List<OptionRateDto>? OptionRates { get; set; }
        public SuspensionDto? Suspensions { get; set; }
        public bool IsAutoRetrySettlement { get; set; }
    }
}
