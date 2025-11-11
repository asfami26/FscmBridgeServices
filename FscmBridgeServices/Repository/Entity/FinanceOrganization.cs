namespace FscmBridgeServices.Repository.Entity
{
    public class FinanceOrganization
    {
        public string? accountNumber { get; set; }
        public string? organizationUuid { get; set; }
        public string? accountName { get; set; }
        public string? currency { get; set; }
        public string? country { get; set; }
        public string? bankName { get; set; }
        public string? city { get; set; }
        public string? swiftCode { get; set; }
    }
}
