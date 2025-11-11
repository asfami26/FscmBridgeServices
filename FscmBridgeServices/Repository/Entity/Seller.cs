namespace FscmBridgeServices.Repository.Entity
{
    public class Seller
    {
        public string? organizationUuid { get; set; }
        public string? organizationName { get; set; }
        public string? code { get; set; }
        public string? type { get; set; }
        public bool isPrincipal { get; set; }
    }
}
