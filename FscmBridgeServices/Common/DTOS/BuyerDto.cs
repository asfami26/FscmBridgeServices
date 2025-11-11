namespace FscmBridgeServices.Common.DTOS
{
    public class BuyerDto
    {
        public string? OrganizationUuid { get; set; }
        public string? OrganizationName { get; set; }
        public string? Code { get; set; }
        public string? Type { get; set; }
        public bool isPrincipal { get; set; }

    }
}
