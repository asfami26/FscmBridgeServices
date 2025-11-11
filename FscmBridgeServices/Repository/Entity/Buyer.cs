namespace FscmBridgeServices.Repository.Entity
{
    public class Buyer
    {
        public string? organizationUuid { get; set; }
        public string? organizationName { get; set; }
        public string? code { get; set; }
        public string? type { get; set; }
        public bool isPrincipal { get; set; }
    }

}
