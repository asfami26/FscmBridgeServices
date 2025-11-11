namespace FscmBridgeServices.Common.DTOS
{
    public class FinanceOrganizationDto
    {
        public string? AccountNumber { get; set; }
        public string? OrganizationUuid { get; set; }
        public string? AccountName { get; set; }
        public string? Currency { get; set; }
        public string? Country { get; set; }
        public string? BankName { get; set; }
        public string? City { get; set; }
        public string? SwiftCode { get; set; }
    }
}
